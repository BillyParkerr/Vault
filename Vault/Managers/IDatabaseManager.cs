using Application.Models;

namespace Application.Managers;

public interface IDatabaseManager
{
    void SaveChanges();

    event EventHandler vaultContentsChangedEvent;

    // EncryptedFile Queries
    EncryptedFile? GetEncryptedFileById(int id);
    EncryptedFile? GetEncryptedFileByFilePath(string filePath);
    List<EncryptedFile> GetAllEncryptedFiles();
    void AddEncryptedFile(string filePath, bool uniquePassword);
    void DeleteEncryptedFileById(int id);
    void DeleteEncryptedFileByFilePath(string filePath);

    // EncryptionKey Queries (Their should only ever be one encryption key!)
    EncryptionKey GetEncryptionKey();
    void ChangeEncryptionKey(string newEncryptionKey);
    void SetEncryptionKey(string encryptionKey);
    bool IsEncryptionKeySet();
}