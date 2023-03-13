using Application.Models;

namespace Application.Managers;

public class DatabaseManager : IDatabaseManager
{
    private SqliteDbContext DbContext;
    public event EventHandler vaultContentsChangedEvent;

    public DatabaseManager()
    {
        DbContext = new SqliteDbContext();
    }

    public void SaveChanges()
    {
        DbContext.SaveChanges();
    }

    // EncryptedFile Queries
    public EncryptedFile? GetEncryptedFileById(int id)
    {
        var encryptedFile = DbContext.EncryptedFiles.FirstOrDefault(x => x.Id == id);
        return encryptedFile;
    }

    public EncryptedFile? GetEncryptedFileByFilePath(string filePath)
    {
        var encryptedFile = DbContext.EncryptedFiles.FirstOrDefault(x => x.FilePath == filePath);
        return encryptedFile;
    }

    public List<EncryptedFile> GetAllEncryptedFiles()
    {
        var encryptedFiles = DbContext.EncryptedFiles.ToList();
        return encryptedFiles;
    }

    public void AddEncryptedFile(string filePath, bool uniquePassword)
    {
        DbContext.Add(new EncryptedFile { FilePath = filePath, UniquePassword = uniquePassword });
        vaultContentsChangedEvent.Invoke(this, EventArgs.Empty);
    }

    public void DeleteEncryptedFileById(int id)
    {
        var encryptedFileToRemove = DbContext.EncryptedFiles.FirstOrDefault(_ => _.Id == id);
        if (encryptedFileToRemove != null)
        {
            DbContext.Remove(encryptedFileToRemove);
            vaultContentsChangedEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeleteEncryptedFileByFilePath(string filePath)
    {
        var encryptedFileToRemove = DbContext.EncryptedFiles.FirstOrDefault(_ => _.FilePath == filePath);
        if (encryptedFileToRemove != null)
        {
            DbContext.Remove(encryptedFileToRemove);
            vaultContentsChangedEvent.Invoke(this, EventArgs.Empty);
        }
    }

    // EncryptionKey Queries (There should only ever be one encryption key)
    public EncryptionKey GetEncryptionKey()
    {
        return DbContext.EncryptionKeys.Single();
    }

    public void ChangeEncryptionKey(string newEncryptionKey)
    {
        DbContext.EncryptionKeys.Single().Key = newEncryptionKey;
    }

    public void SetEncryptionKey(string encryptionKey)
    {
        DbContext.EncryptionKeys.Add(new EncryptionKey { Key = encryptionKey });
    }

    public bool IsEncryptionKeySet()
    {
        var key = DbContext.EncryptionKeys.FirstOrDefault();
        return key != null;
    }
}