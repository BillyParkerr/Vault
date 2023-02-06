using Application.Models;

namespace Application.Managers;

public interface IFileManager
{
    List<EncryptedFile> GetAllFilesInVault();
    bool AddFileToVault(string filePath, string? password = null);
    bool DownloadFromFromVault(string encryptedFilePath, string destinationFilePath, string? password = null);
}