using Application.Models;

namespace Application.Managers;

public class DatabaseManager : IDatabaseManager
{
    private SqliteDbContext _dbContext;
    public event EventHandler VaultContentsChangedEvent;

    public void SetSqliteDbContextIfNotExisits(SqliteDbContext dbContext = null)
    {
        if (_dbContext != null)
        {
            return;
        }

        if (dbContext == null)
        {
            _dbContext = new();
        }
        else
        {
            _dbContext = dbContext;
        }
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    // EncryptedFile Queries
    public EncryptedFile GetEncryptedFileById(int id)
    {
        var encryptedFile = _dbContext.EncryptedFiles.FirstOrDefault(x => x.Id == id);
        return encryptedFile;
    }

    public EncryptedFile GetEncryptedFileByFilePath(string filePath)
    {
        var encryptedFile = _dbContext.EncryptedFiles.FirstOrDefault(x => x.FilePath == filePath);
        return encryptedFile;
    }

    public List<EncryptedFile> GetAllEncryptedFiles()
    {
        var encryptedFiles = _dbContext.EncryptedFiles.ToList();
        return encryptedFiles;
    }

    public void AddEncryptedFile(string filePath, bool uniquePassword)
    {
        _dbContext.Add(new EncryptedFile { FilePath = filePath, UniquePassword = uniquePassword });
        VaultContentsChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void DeleteEncryptedFileById(int id)
    {
        var encryptedFileToRemove = _dbContext.EncryptedFiles.FirstOrDefault(_ => _.Id == id);
        if (encryptedFileToRemove != null)
        {
            _dbContext.Remove(encryptedFileToRemove);
            VaultContentsChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeleteEncryptedFileByFilePath(string filePath)
    {
        var encryptedFileToRemove = _dbContext.EncryptedFiles.FirstOrDefault(_ => _.FilePath == filePath);
        if (encryptedFileToRemove != null)
        {
            _dbContext.Remove(encryptedFileToRemove);
            VaultContentsChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    // EncryptionKey Queries (There should only ever be one encryption key)
    public EncryptionKey GetEncryptionKey()
    {
        return _dbContext.EncryptionKeys.Single();
    }

    public void ChangeEncryptionKey(string newEncryptionKey)
    {
        _dbContext.EncryptionKeys.Single().Key = newEncryptionKey;
    }

    public void SetEncryptionKey(string encryptionKey)
    {
        _dbContext.EncryptionKeys.Add(new() { Key = encryptionKey });
    }

    public bool IsEncryptionKeySet()
    {
        var key = _dbContext.EncryptionKeys.FirstOrDefault();
        return key != null;
    }
}