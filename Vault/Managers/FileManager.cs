using Application.Models;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Application.Managers;

public class FileManager : IFileManager
{
    private readonly IEncryptionManager _encryptionManager;
    private readonly IDatabaseManager _databaseManager;
    private readonly IFileMonitoringManager _fileMonitoringManager;
    private readonly AppSettings _appSettings;

    public FileManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager, IFileMonitoringManager fileMonitoringManager, AppSettings appSettings)
    {
        _encryptionManager = encryptionManager;
        _databaseManager = databaseManager;
        _fileMonitoringManager = fileMonitoringManager;
        _appSettings = appSettings;
    }

    /// <summary>
    /// Gets a list of all the files currently in the vault that do not use a unique password.
    /// </summary>
    /// <returns>List of all files in the Vault.</returns>
    public List<EncryptedFile> GetAllFilesInVault()
    {
        // Ensure that the files are up to date.
        _databaseManager.SaveChanges();
        var encryptedFiles = _databaseManager.GetAllEncryptedFiles();

        foreach (var encryptedFile in encryptedFiles)
        {
            var decryptedFile = _encryptionManager.DecryptString(Path.GetFileNameWithoutExtension(encryptedFile.FilePath));
            FileInformation fileInformation = new()
            {
                FileName = Path.GetFileNameWithoutExtension(decryptedFile),
                FileSize = FormatFileSize(new FileInfo(encryptedFile.FilePath).Length.ToString()),
                FileExtension = Path.GetExtension(decryptedFile)
            };
            encryptedFile.DecryptedFileInformation = fileInformation;
        }

        return encryptedFiles;
    }

    /// <summary>
    /// Adds a file to the vault by encrypting the file then adding the information to the database.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True if success, False if failure.</returns>
    public bool AddFileToVault(string filePath)
    {
        try
        {
            // Encrypt File
            string encryptedFilePath = _encryptionManager.EncryptFile(filePath);

            // Add file to database
            _databaseManager.AddEncryptedFile(encryptedFilePath);

            // If the user has chosen to delete the original file once it has been added to the vault
            // we will proceed to delete the file.
            if (_appSettings.DeleteUnencryptedFileUponUpload)
            {
                File.Delete(filePath);
            }

            _databaseManager.SaveChanges();
        }
        catch (Exception ex)
        {
            // TODO Potentially add logging here?
            return false;
        }

        return true;
    }

    /// <summary>
    /// Decrypts and downloads an encrypted file from the vault to a given destination.
    /// </summary>
    /// <param name="encryptedFilePath"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="password"></param>
    /// <returns>True upon success, False upon failure</returns>
    public bool DownloadFileFromVault(string encryptedFilePath, string destinationFilePath, string password = null)
    {
        try
        {
            var decryptedFilePath = _encryptionManager.DecryptFile(encryptedFilePath, destinationFilePath);
            if (!File.Exists(decryptedFilePath))
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes the file from the system and removes the EncryptedFile from the database.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True upon success, False upon failure</returns>
    public bool DeleteFileFromVault(string filePath)
    {
        try
        {
            File.Delete(filePath);
            if (File.Exists(filePath))
            {
                return false;
            }
            else
            {
                _databaseManager.DeleteEncryptedFileByFilePath(filePath);
                _databaseManager.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes any Files that were created for temporary purposes suchas opening a file within the vault.
    /// </summary>
    public void CleanupTempFiles()
    {
        foreach (string filePath in Directory.GetFiles(DirectoryPaths.DecryptedFilesTempDirectory))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                // TODO Add logging
            }
        }
    }

    /// <summary>
    /// Decrypts a file within the vault to a temporary location and initilises the file monitering process.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True upon success, False upon failure</returns>
    public bool OpenFileFromVaultAndReencryptUponClosure(string filePath)
    {
        try
        {
            var destinationFileLocation = GetTempFileDestinationLocation(filePath);
            if (!File.Exists(destinationFileLocation))
            {
                _encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
            }

            if (File.Exists(destinationFileLocation))
            {
                _fileMonitoringManager.Initilise(destinationFileLocation, filePath);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private string GetTempFileDestinationLocation(string filePath, string password = null)
    {
        var destinationFileName = _encryptionManager.DecryptString(Path.GetFileNameWithoutExtension(filePath), password);
        return Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, destinationFileName);
    }

    /// <summary>
    /// Adds a fodler to the vault by zipping then encrypting the folder then adding the information to the database.
    /// Uses the default password unless a password is given.
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="password"></param>
    /// <returns>True if success, False if failure.</returns>
    public bool ZipFolderAndAddToVault(string folderPath)
    {
        try
        {
            // Create a unique file name for the zip file in the temp directory
            string zipFileName = Path.GetFileName(folderPath) + ".zip";
            string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

            // Create a new ZIP file and add the contents of the folder to it
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            bool success = AddFileToVault(zipFilePath);
            return success;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Downloads a encrypted file to a specified destination.
    /// The file is first decrypted then reencrypted with the given password.
    /// The decrypted copy is then deleted.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="newEncryptionPassword"></param>
    /// <returns>True upon success or False upon failure</returns>
    public bool DownloadEncryptedFileFromVault(string filePath, string destinationFilePath, string newEncryptionPassword)
    {
        try
        {
            // The first check is necassary as the file may be already decrypted in the temporary location due to the user opening the file.
            var decryptedFilePath = GetTempFileDestinationLocation(filePath);
            if (!File.Exists(decryptedFilePath))
            {
                decryptedFilePath = _encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
            }

            if (decryptedFilePath == null || !File.Exists(decryptedFilePath))
            {
                return false;
            }

            var encryptedFileLocation = _encryptionManager.EncryptFile(decryptedFilePath, newEncryptionPassword);

            if (encryptedFileLocation == null || !File.Exists(encryptedFileLocation))
            {
                return false;
            }

            var destinationFileName = Path.Combine(destinationFilePath, Path.GetFileName(encryptedFileLocation));
            File.Copy(encryptedFileLocation, destinationFileName);

            File.Delete(encryptedFileLocation);
            if (!File.Exists(destinationFileName))
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Imports an existing EncryptedFile. This follows the following process:
    /// 
    /// 1 - Decrpyt the encrypted file to a temp location.
    /// 2 - Encrypted the decrypted file from the temp location.
    /// 3 - Add the encrypted file to the database.
    /// 4 - Delete the temp decrypted file.
    /// </summary>
    /// <param name="filePath">File path for the encrypted file.</param>
    /// <param name="encryptionPassword">Password which the given file was encrypted with.</param>
    /// <returns>True upon success or False upon failure</returns>
    public bool ImportEncryptedFileToVault(string filePath, string encryptionPassword)
    {
        try
        {
            var decryptedFilePath = GetTempFileDestinationLocation(filePath, encryptionPassword);
            if (!File.Exists(decryptedFilePath))
            {
                decryptedFilePath = _encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
            }

            if (decryptedFilePath == null || !File.Exists(decryptedFilePath))
            {
                return false;
            }

            var success = AddFileToVault(decryptedFilePath);

            File.Delete(decryptedFilePath);

            return success;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a protected file containing the given password. If a protected file already exists this is deleted.
    /// </summary>
    /// <param name="password"></param>
    public void ProtectAndSavePassword(string password)
    {
        if (ProtectedPasswordExists())
        {
            File.Delete(DirectoryPaths.EncryptedKeyPath);
        }

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] encryptedPassword = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes(DirectoryPaths.EncryptedKeyPath, encryptedPassword);
    }

    /// <summary>
    /// Decrypts and reads the protected file.
    /// </summary>
    /// <returns>Password from the protected file.</returns>
    public string ReadAndReturnProtectedPassword()
    {
        if (!ProtectedPasswordExists())
        {
            return null;
        }

        byte[] encryptedPassword = File.ReadAllBytes(DirectoryPaths.EncryptedKeyPath);
        byte[] decryptedPasswordBytes = ProtectedData.Unprotect(encryptedPassword, null, DataProtectionScope.CurrentUser);
        string decryptedPassword = Encoding.UTF8.GetString(decryptedPasswordBytes);
        return decryptedPassword;
    }

    /// <summary>
    /// Checks if an protected password file exists.
    /// </summary>
    /// <returns>True if exists, False if not</returns>
    public bool ProtectedPasswordExists()
    {
        return File.Exists(DirectoryPaths.EncryptedKeyPath);
    }

    /// <summary>
    /// Executes the specified function on a Single Thread Apartment (STA) thread.
    /// This method should be used when interacting with components that require
    /// the STA threading model, such as OpenFileDialog, FolderBrowserDialog, and
    /// some COM components.
    /// </summary>
    /// <param name="func">The function to execute on the STA thread.</param>
    private static T RunOnSTAThread<T>(Func<T> func)
    {
        var result = default(T);
        var thread = new Thread(() => { result = func(); });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        return result;
    }

    /// <summary>
    /// Displays a file picker dialog to the user and returns the selected file path.
    /// This method ensures that the file picker dialog is run on an STA thread, as required.
    /// </summary>
    /// <param name="filter">Optional file filter string to show only specific file types in the dialog.</param>
    /// <returns>The file path of the selected file, or null if no file was selected.</returns>
    public string GetFilePathFromExplorer(string filter = null)
    {
        return RunOnSTAThread(() =>
        {
            string result = null;
            OpenFileDialog openFileDialog1 = new()
            {
                Title = "Select File",
                InitialDirectory = @"C:\", //--"C:\\";
                Filter = filter,
                FilterIndex = 1,
                Multiselect = false,
                CheckFileExists = true
            };

            openFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                result = openFileDialog1.FileName;
            }

            return result;
        });
    }

    /// <summary>
    /// Displays a folder picker dialog to the user and returns the selected folder path.
    /// This method ensures that the folder picker dialog is run on an STA thread, as required.
    /// </summary>
    /// <returns>The folder path of the selected folder, or null if no folder was selected.</returns>
    public string GetFolderPathFromExplorer()
    {
        return RunOnSTAThread(() =>
        {
            FolderBrowserDialog folderBrowserDialog = new()
            {
                Description = "Select Folder",
                InitialDirectory = _appSettings.DefaultDownloadLocation
            };

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        });
    }

    /// <summary>
    /// Opens Windows Explorer and selects the specified file or folder.
    /// </summary>
    /// <param name="path">The path of the file or folder to be selected in Windows Explorer.</param>
    public void OpenFolderInExplorer(string path)
    {
        string argument = "/select, \"" + path + "\"";
        System.Diagnostics.Process.Start("explorer.exe", argument);
    }

    private static string FormatFileSize(string fileLength)
    {
        if (string.IsNullOrEmpty(fileLength))
        {
            return fileLength;
        }

        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = Convert.ToDouble(fileLength);
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}