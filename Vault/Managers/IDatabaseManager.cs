using Application.Models;

namespace Application.Managers;

public interface IDatabaseManager
{
    /// <summary>
    /// Sets the DatabaseManager SqliteDbContext field provided it is not already populated.
    ///
    /// This allows unit tests to provide an in memory databsae for the SqliteDbContext.
    /// </summary>
    /// <param name="dbContext"></param>
    void SetSqliteDbContextIfNotExisits(SqliteDbContext dbContext = null);

    /// <summary>
    /// Commit any changes to the database.
    /// </summary>
    void SaveChanges();

    event EventHandler VaultContentsChangedEvent;

    #region EncryptedFile Queries

    /// <summary>
    /// Attempts to find an EncryptedFile with a given file path.
    /// </summary>
    /// <param name="filePath">The file path of the EncryptedFile to find.</param>
    /// <returns>EncryptedFile or null if none found.</returns>
    EncryptedFile GetEncryptedFileByFilePath(string filePath);

    /// <summary>
    /// Gets a list of all EncryptedFiles in the database.
    /// </summary>
    /// <returns>A list of all EncryptedFiles in the database.</returns>
    List<EncryptedFile> GetAllEncryptedFiles();

    /// <summary>
    /// Creates and adds an EncryptedFile to the database.
    /// </summary>
    /// <param name="filePath"></param>
    void AddEncryptedFile(string filePath);

    /// <summary>
    /// Attempts to delete an EncryptedFile with a given file path.
    /// </summary>
    /// <param name="filePath">The file path of the EncryptedFile to delete.</param>
    void DeleteEncryptedFileByFilePath(string filePath);

    #endregion

    #region EncryptionKey Queries

    /// <summary>
    /// Gets the EncryptionKey from the database.
    ///
    /// There should only ever be one encryption key.
    /// </summary>
    /// <returns>The EncryptionKey.</returns>
    EncryptionKey GetEncryptionKey();

    /// <summary>
    /// Changes the existing EncryptionKey with a new EncryptionKey
    /// </summary>
    /// <param name="newEncryptionKey"></param>
    void ChangeEncryptionKey(string newEncryptionKey);

    /// <summary>
    /// Adds a new EncryptionKey to the database.
    ///
    /// WARNING - Should only be used when it is certain an EncryptionKey does not exist!
    ///           If an EncryptionKey is present an Exception will be thrown!
    /// </summary>
    /// <param name="encryptionKey"></param>
    void SetEncryptionKey(string encryptionKey);

    /// <summary>
    /// Checks wether an EncryptionKey is present in the database.
    /// </summary>
    /// <returns>True if present, False if not</returns>
    bool IsEncryptionKeySet();

    #endregion
}