using Application.Models;

namespace Application.Managers;

public class FileManager : IFileManager
{
    private string EncryptedFilesPath = @"C:\Users\Billy\AppData\Roaming\PersonalVaultApplication\EncryptedFiles\Common"; // TODO Move this into a base class
    private IEncryptionManager encryptionManager;
    private IDatabaseManager databaseManager;

    public FileManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager)
    {
        this.encryptionManager = encryptionManager;
        this.databaseManager = databaseManager;
    }

    /// <summary>
    /// Gets a list of all the files currently in the vault that do not use a unique password.
    /// </summary>
    /// <returns>List of all files in the Vault.</returns>
    public List<EncryptedFile> GetAllFilesInVault()
    {
        var encryptedFiles = databaseManager.GetAllEncryptedFiles();

        foreach (var encryptedFile in encryptedFiles.Where(_ => _.UniquePassword == false))
        {
            FileInformation fileInformation = new FileInformation
            {
                FileName = encryptionManager.DecryptFileName(Path.GetFileNameWithoutExtension(encryptedFile.FilePath), LoginInfomation.Password),
                FileSize = new FileInfo(encryptedFile.FilePath).Length.ToString(),
                FileExtension = Path.GetExtension(encryptedFile.FilePath)
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
    public bool AddFileToVault(string filePath, string? password = null)
    {
        try
        {
            // Encrypt File
            string encryptedFilePath = encryptionManager.EncryptFile(filePath, password);

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

    public bool DownloadFromFromVault(string encryptedFilePath, string destinationFilePath, string? password = null)
    {
        var decryptedFilePath = encryptionManager.DecryptFile(encryptedFilePath, password, destinationFilePath);
        if (!File.Exists(decryptedFilePath))
        {
            return false;
        }

        return true;
    }
}