using Application.Managers;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.ManagerTests;

public class DatabaseManagerTests
{
    private SqliteDbContext _dbContext;
    private DatabaseManager _databaseManager;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SqliteDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;
        _dbContext = new SqliteDbContext(options);
        _databaseManager = new DatabaseManager();
        _databaseManager.SetSqliteDbContextIfNotExisits(_dbContext);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    [Test]
    public void AddEncryptedFile_AddsEncryptedFile()
    {
        // Arrange
        var filePath = "test.txt";

        // Act
        _databaseManager.AddEncryptedFile(filePath);
        _databaseManager.SaveChanges();
        var result = _databaseManager.GetEncryptedFileByFilePath(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(filePath, result.FilePath);
    }

    [Test]
    public void DeleteEncryptedFileByFilePath_DeletesEncryptedFile()
    {
        // Arrange
        var filePath = "test.txt";
        var encryptedFile = new EncryptedFile { FilePath = filePath };
        _dbContext.EncryptedFiles.Add(encryptedFile);
        _dbContext.SaveChanges();

        // Act
        _databaseManager.DeleteEncryptedFileByFilePath(filePath);
        _databaseManager.SaveChanges();
        var result = _databaseManager.GetEncryptedFileByFilePath(filePath);

        // Assert
        Assert.Null(result);
    }

    [Test]
    public void SetEncryptionKey_SetsEncryptionKey()
    {
        // Arrange
        var encryptionKey = "test123";

        // Act
        _databaseManager.SetEncryptionKey(encryptionKey);
        _databaseManager.SaveChanges();
        var result = _databaseManager.GetEncryptionKey();

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(encryptionKey, result.Key);
    }

    [Test]
    public void ChangeEncryptionKey_ChangesEncryptionKey()
    {
        // Arrange
        var encryptionKey = "test123";
        var newEncryptionKey = "newTest123";
        _databaseManager.SetEncryptionKey(encryptionKey);
        _databaseManager.SaveChanges();

        // Act
        _databaseManager.ChangeEncryptionKey(newEncryptionKey);
        _databaseManager.SaveChanges();
        var result = _databaseManager.GetEncryptionKey();

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(newEncryptionKey, result.Key);
    }

    [Test]
    public void IsEncryptionKeySet_ReturnsTrueWhenSet()
    {
        // Arrange
        var encryptionKey = "test123";
        _databaseManager.SetEncryptionKey(encryptionKey);
        _databaseManager.SaveChanges();

        // Act
        var result = _databaseManager.IsEncryptionKeySet();

        // Assert
        Assert.True(result);
    }

    [Test]
    public void IsEncryptionKeySet_ReturnsFalseWhenNotSet()
    {
        // Act
        var result = _databaseManager.IsEncryptionKeySet();

        // Assert
        Assert.False(result);
    }
}