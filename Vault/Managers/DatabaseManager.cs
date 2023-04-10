using Application.Models;

namespace Application.Managers;

public class DatabaseManager : IDatabaseManager
{
    private SqliteDbContext _dbContext;
    public event EventHandler VaultContentsChangedEvent;

    /// <summary>
    /// Sets the DatabaseManager SqliteDbContext field provided it is not already populated.
    ///
    /// This allows unit tests to provide an in memory databsae for the SqliteDbContext.
    /// </summary>
    /// <param name="dbContext"></param>
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

    /// <summary>
    /// Commit any changes to the database.
    /// </summary>
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    #region EncryptedFile Queries

    /// <summary>
    /// Attempts to find an EncryptedFile with a given file path.
    /// </summary>
    /// <param name="filePath">The file path of the EncryptedFile to find.</param>
    /// <returns>EncryptedFile or null if none found.</returns>
    public EncryptedFile GetEncryptedFileByFilePath(string filePath)
    {
        var encryptedFile = _dbContext.EncryptedFiles.FirstOrDefault(x => x.FilePath == filePath);
        return encryptedFile;
    }

    /// <summary>
    /// Gets a list of all EncryptedFiles in the database.
    /// </summary>
    /// <returns>A list of all EncryptedFiles in the database.</returns>
    public List<EncryptedFile> GetAllEncryptedFiles()
    {
        var encryptedFiles = _dbContext.EncryptedFiles.ToList();
        return encryptedFiles;
    }

    /// <summary>
    /// Creates and adds an EncryptedFile to the database.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="uniquePassword"></param>
    public void AddEncryptedFile(string filePath, bool uniquePassword)
    {
        _dbContext.Add(new EncryptedFile { FilePath = filePath, UniquePassword = uniquePassword });
        VaultContentsChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Attempts to delete an EncryptedFile with a given file path.
    /// </summary>
    /// <param name="filePath">The file path of the EncryptedFile to delete.</param>
    public void DeleteEncryptedFileByFilePath(string filePath)
    {
        var encryptedFileToRemove = _dbContext.EncryptedFiles.FirstOrDefault(_ => _.FilePath == filePath);
        if (encryptedFileToRemove != null)
        {
            _dbContext.Remove(encryptedFileToRemove);
            VaultContentsChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion

    #region EncryptionKey Queries

    /// <summary>
    /// Gets the EncryptionKey from the database.
    ///
    /// There should only ever be one encryption key.
    /// </summary>
    /// <returns>The EncryptionKey.</returns>
    public EncryptionKey GetEncryptionKey()
    {
        return _dbContext.EncryptionKeys.Single();
    }

    /// <summary>
    /// Changes the existing EncryptionKey with a new EncryptionKey
    /// </summary>
    /// <param name="newEncryptionKey"></param>
    public void ChangeEncryptionKey(string newEncryptionKey)
    {
        _dbContext.EncryptionKeys.Single().Key = newEncryptionKey;
    }

    /// <summary>
    /// Adds a new EncryptionKey to the database.
    ///
    /// WARNING - Should only be used when it is certain an EncryptionKey does not exist!
    ///           If an EncryptionKey is present an Exception will be thrown!
    /// </summary>
    /// <param name="encryptionKey"></param>
    public void SetEncryptionKey(string encryptionKey)
    {
        if (IsEncryptionKeySet())
        {
            throw new InvalidOperationException("A new EncryptionKey cannot be added when one is already present! Please use ChangeEncryptionKey instead.");
        }

        _dbContext.EncryptionKeys.Add(new() { Key = encryptionKey });
    }

    /// <summary>
    /// Checks wether an EncryptionKey is present in the database.
    /// </summary>
    /// <returns>True if present, False if not</returns>
    public bool IsEncryptionKeySet()
    {
        var key = _dbContext.EncryptionKeys.FirstOrDefault();
        return key != null;
    }

    #endregion
}