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
    public void TestAddAndGetEncryptedFile()
    {
        // Arrange
        var filePath = "test.txt";
        var uniquePassword = true;

        // Act
        _databaseManager.AddEncryptedFile(filePath, uniquePassword);
        _databaseManager.SaveChanges();
        var result = _databaseManager.GetEncryptedFileByFilePath(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(filePath, result.FilePath);
        Assert.AreEqual(uniquePassword, result.UniquePassword);
    }

    [Test]
    public void TestDeleteEncryptedFile()
    {
        // Arrange
        var filePath = "test.txt";
        var uniquePassword = true;
        var encryptedFile = new EncryptedFile { FilePath = filePath, UniquePassword = uniquePassword };
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
    public void TestSetAndGetEncryptionKey()
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
    public void TestChangeEncryptionKey()
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
    public void TestIsEncryptionKeySet()
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
}