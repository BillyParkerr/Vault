using Application.Models;

namespace Application.Managers;

public interface IFileManager
{
    List<EncryptedFile> GetAllFilesInVault();
    bool AddFileToVault(string filePath, string password = null);
    bool ZipFolderAndAddToVault(string folderPath, string password = null);
    bool DownloadFileFromVault(string encryptedFilePath, string destinationFilePath, string password = null);
    bool DeleteFileFromVault(string filePath);
    bool OpenFileFromVaultAndReencryptUponClosure(string filePath);
    bool DownloadEncryptedFileFromVault(string filePath, string destinationFilePath, string newEncryptionPassword);
    bool ImportEncryptedFileToVault(string filePath, string encryptionPassword);
    void ProtectAndSavePassword(string password);
    string ReadAndReturnProtectedPassword();
    bool ProtectedPasswordExists();
    void CleanupTempFiles();
    string GetFilePathFromExplorer(string filter = null);
    string GetFolderPathFromExplorer();
    void OpenFolderInExplorer(string path);
}