using Application.Managers;
using Application.Models;
using Moq;
using System.Text;

namespace Application.Tests.ManagerTests;

public class EncryptionManagerTests
{
    private Mock<IDatabaseManager> _databaseManagerMock;
    private EncryptionManager _encryptionManager;
    private TestHelper _testHelper;
    private List<string> filesToDelete;

    [SetUp]
    public void SetUp()
    {
        _databaseManagerMock = new Mock<IDatabaseManager>();
        _encryptionManager = new EncryptionManager(_databaseManagerMock.Object);
        _testHelper = new TestHelper();
        filesToDelete = new List<string>();
    }

    [TearDown]
    public void Cleanup()
    {
        // If any test creates files on the system, delete them here.
        foreach (var file in filesToDelete)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

    [Test]
    public void SetEncryptionPassword_WhenPasswordIsCorrect_AllowsEncryptionAndDecryption()
    {
        // Arrange
        string password = "test_password";
        string encryptedKey = _encryptionManager.EncryptString("test_encryption_key", password);

        _databaseManagerMock.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = encryptedKey });

        // Act
        _encryptionManager.SetEncryptionPassword(password);
        string plainText = "This is a test message.";
        string encryptedText = _encryptionManager.EncryptString(plainText);
        string decryptedText = _encryptionManager.DecryptString(encryptedText);

        // Assert
        Assert.AreEqual(plainText, decryptedText);
    }


    [Test]
    public void EncryptString_DecryptString_EncryptsAndDecryptsCorrectly()
    {
        // Arrange
        string plaintext = "test_data";
        string password = "test_password";

        // Act
        string encryptedText = _encryptionManager.EncryptString(plaintext, password);
        string decryptedText = _encryptionManager.DecryptString(encryptedText, password);

        // Assert
        Assert.AreNotEqual(plaintext, encryptedText);
        Assert.AreEqual(plaintext, decryptedText);
    }

    [Test]
    public void EncryptString_DecryptString_WithValidPassword_ReturnsOriginalString()
    {
        // Arrange
        string testString = "This is a test string.";
        string password = "test_password";

        // Act
        string encryptedString = _encryptionManager.EncryptString(testString, password);
        string decryptedString = _encryptionManager.DecryptString(encryptedString, password);

        // Assert
        Assert.AreEqual(testString, decryptedString);
    }

    [Test]
    public void EncryptString_DecryptString_WithDefaultPassword_ReturnsOriginalString()
    {
        // Arrange
        string testString = "This is a test string.";
        string password = "test_password";
        SetEncryptionKeyBasedOnPassword(password);
        _encryptionManager.SetEncryptionPassword(password);

        // Act
        string encryptedString = _encryptionManager.EncryptString(testString);
        string decryptedString = _encryptionManager.DecryptString(encryptedString);

        // Assert
        Assert.AreEqual(testString, decryptedString);
    }

    [Test]
    public void EncryptFile_DecryptFile_WithValidPassword_ReturnsOriginalFileContent()
    {
        // Arrange
        string sourceFilePath = _testHelper.CreateTextFile();
        filesToDelete.Add(sourceFilePath);
        string password = "test_password";

        // Act
        string encryptedFilePath = _encryptionManager.EncryptFile(sourceFilePath, password);
        string decryptedFilePath = _encryptionManager.DecryptFile(encryptedFilePath, null, password);
        filesToDelete.Add(encryptedFilePath);
        filesToDelete.Add(decryptedFilePath);

        // Assert
        string originalContent = File.ReadAllText(sourceFilePath);
        string decryptedContent = File.ReadAllText(decryptedFilePath);
        Assert.AreEqual(originalContent, decryptedContent);
    }

    [Test]
    public void EncryptFile_DecryptFile_WithDefaultPassword_ReturnsOriginalFileContent()
    {
        // Arrange
        string sourceFilePath = _testHelper.CreateTextFile();
        filesToDelete.Add(sourceFilePath);
        string password = "test_password";
        SetEncryptionKeyBasedOnPassword(password);
        _encryptionManager.SetEncryptionPassword(password);

        // Act
        string encryptedFilePath = _encryptionManager.EncryptFile(sourceFilePath);
        string decryptedFilePath = _encryptionManager.DecryptFile(encryptedFilePath);
        filesToDelete.Add(encryptedFilePath);
        filesToDelete.Add(decryptedFilePath);

        // Assert
        string originalContent = File.ReadAllText(sourceFilePath);
        string decryptedContent = File.ReadAllText(decryptedFilePath);
        Assert.AreEqual(originalContent, decryptedContent);
    }

    private void SetEncryptionKeyBasedOnPassword(string password)
    {
        var baseString = GenerateRandomStringForEncryptionKey();
        var encryptedBaseString = _encryptionManager.EncryptString(baseString, password);
        var encryptionKey = new EncryptionKey
        {
            Key = encryptedBaseString
        };

        _databaseManagerMock.Setup(x => x.GetEncryptionKey()).Returns(encryptionKey);
    }

    private static string GenerateRandomStringForEncryptionKey()
    {
        Random random = new();
        int length = random.Next(20, 26);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder stringBuilder = new(length);
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}