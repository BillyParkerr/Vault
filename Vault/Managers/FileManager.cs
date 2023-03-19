using Application.Models;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Application.Managers;

public class FileManager : IFileManager
{
    private IEncryptionManager encryptionManager;
    private IDatabaseManager databaseManager;
    private IFileMonitoringManager fileMonitoringManager;

    public FileManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager, IFileMonitoringManager fileMonitoringManager)
    {
        this.encryptionManager = encryptionManager;
        this.databaseManager = databaseManager;
        this.fileMonitoringManager = fileMonitoringManager;
    }

    /// <summary>
    /// Gets a list of all the files currently in the vault that do not use a unique password.
    /// </summary>
    /// <returns>List of all files in the Vault.</returns>
    public List<EncryptedFile> GetAllFilesInVault()
    {
        // Ensure that the files are up to date.
        databaseManager.SaveChanges();
        var encryptedFiles = databaseManager.GetAllEncryptedFiles();

        foreach (var encryptedFile in encryptedFiles.Where(_ => _.UniquePassword == false))
        {
            var decryptedFile = encryptionManager.DecryptString(Path.GetFileNameWithoutExtension(encryptedFile.FilePath));
            FileInformation fileInformation = new FileInformation
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
    /// This will use the default encryption password.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True if success, False if failure.</returns>
    public bool AddFileToVault(string filePath, string password = null)
    {
        try
        {
            // Encrypt File
            string encryptedFilePath = encryptionManager.EncryptFile(filePath);

            // Add file to database
            if (password == null)
            {
                databaseManager.AddEncryptedFile(encryptedFilePath, false);
            }
            else
            {
                databaseManager.AddEncryptedFile(encryptedFilePath, true);
            }

            databaseManager.SaveChanges();
        }
        catch (Exception ex)
        {
            // TODO Potentially add logging here?
            return false;
        }

        return true;
    }

    public bool DownloadFileFromVault(string encryptedFilePath, string destinationFilePath, string? password = null)
    {
        try
        {
            var decryptedFilePath = encryptionManager.DecryptFile(encryptedFilePath, destinationFilePath);
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
                databaseManager.DeleteEncryptedFileByFilePath(filePath);
                databaseManager.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

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

    public bool OpenFileFromVaultAndReencryptUponClosure(string filePath)
    {
        try
        {
            var destinationFileLocation = GetTempFileDestinationLocation(filePath);
            if (!File.Exists(destinationFileLocation))
            {
                encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
            }

            if (File.Exists(destinationFileLocation))
            {
                fileMonitoringManager.Initilise(destinationFileLocation, filePath);
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
        var destinationFileName = encryptionManager.DecryptString(Path.GetFileNameWithoutExtension(filePath), password);
        return Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, destinationFileName);
    }

    public bool ZipFolderAndAddToVault(string folderPath, string password = null)
    {
        try
        {
            // Create a unique file name for the zip file in the temp directory
            string zipFileName = Path.GetFileName(folderPath) + ".zip";
            string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

            // Create a new ZIP file and add the contents of the folder to it
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            bool success = AddFileToVault(zipFilePath, password);
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
                decryptedFilePath = encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
            }

            if (decryptedFilePath == null || !File.Exists(decryptedFilePath))
            {
                return false;
            }

            var encryptedFileLocation = encryptionManager.EncryptFile(decryptedFilePath, newEncryptionPassword);

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

    public bool ImportEncryptedFileToVault(string filePath, string encryptionPassword)
    {
        try
        {
            var decryptedFilePath = GetTempFileDestinationLocation(filePath, encryptionPassword);
            if (!File.Exists(decryptedFilePath))
            {
                decryptedFilePath = encryptionManager.DecryptFile(filePath, DirectoryPaths.DecryptedFilesTempDirectory);
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

    public void ProtectAndSavePassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] encryptedPassword = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes(DirectoryPaths.EncryptedKeyPath, encryptedPassword);
    }

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

    public bool ProtectedPasswordExists()
    {
        return File.Exists(DirectoryPaths.EncryptedKeyPath);
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