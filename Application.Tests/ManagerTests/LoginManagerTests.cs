using Application.Enums;
using Application.Managers;
using Application.Models;
using Moq;

namespace Application.Tests.ManagerTests;

[TestFixture]
public class LoginManagerTests
{
    private Mock<IDatabaseManager> _databaseManager;
    private Mock<IEncryptionManager> _encryptionManager;
    private Mock<IFileManager> _fileManager;
    private AppSettings _appSettings;
    private LoginManager _loginManager;

    [SetUp]
    public void Setup()
    {
        _databaseManager = new Mock<IDatabaseManager>();
        _encryptionManager = new Mock<IEncryptionManager>();
        _fileManager = new Mock<IFileManager>();
        _appSettings = new AppSettings { AuthenticationMethod = AuthenticationMethod.Password };
        _loginManager = new LoginManager(_databaseManager.Object, _encryptionManager.Object, _fileManager.Object, _appSettings);
    }

    [Test]
    public void VerifyPassword_ReturnsTrue_WhenPasswordIsValid()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string fakeEncryptedKey = "EncryptedKey";
        _databaseManager.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = fakeEncryptedKey });
        _encryptionManager.Setup(x => x.DecryptString(fakeEncryptedKey, testPassword)).Returns("DecryptedKey");

        // Act
        bool result = _loginManager.VerifyPassword(testPassword);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void VerifyPassword_ReturnsFalse_WhenPasswordIsInvalid()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string fakeEncryptedKey = "EncryptedKey";
        _databaseManager.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = fakeEncryptedKey });
        _encryptionManager.Setup(x => x.DecryptString(fakeEncryptedKey, testPassword)).Throws(new InvalidOperationException());

        // Act
        bool result = _loginManager.VerifyPassword(testPassword);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void SetPassword_CallsEncryptionAndDatabaseMethods()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string fakeEncryptedKey = "EncryptedKey";

        _encryptionManager.Setup(x => x.EncryptString(It.IsAny<string>(), testPassword)).Returns(fakeEncryptedKey);

        // Act
        _loginManager.SetPassword(testPassword);

        // Assert
        _encryptionManager.Verify(x => x.EncryptString(It.IsAny<string>(), testPassword), Times.Once);
        _databaseManager.Verify(x => x.SetEncryptionKey(fakeEncryptedKey), Times.Once);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void ChangePassword_ReturnsFalse_WhenOldPasswordIsInvalid()
    {
        // Arrange
        string oldPassword = "OldPassword123";
        string newPassword = "NewPassword123";
        string fakeEncryptedKey = "EncryptedKey";

        _databaseManager.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = fakeEncryptedKey });
        _encryptionManager.Setup(x => x.DecryptString(fakeEncryptedKey, oldPassword)).Throws(new InvalidOperationException());

        // Act
        bool result = _loginManager.ChangePassword(newPassword, oldPassword);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void ChangePassword_ReturnsTrue_AndUpdatesEncryptionKey_WhenOldPasswordIsValid()
    {
        // Arrange
        string oldPassword = "OldPassword123";
        string newPassword = "NewPassword123";
        string fakeEncryptedKey = "EncryptedKey";
        string decryptedKey = "DecryptedKey";
        string newEncryptedKey = "NewEncryptedKey";

        _databaseManager.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = fakeEncryptedKey });
        _encryptionManager.Setup(x => x.DecryptString(fakeEncryptedKey, oldPassword)).Returns(decryptedKey);
        _encryptionManager.Setup(x => x.EncryptString(decryptedKey, newPassword)).Returns(newEncryptedKey);

        // Act
        bool result = _loginManager.ChangePassword(newPassword, oldPassword);

        // Assert
        Assert.IsTrue(result);
        _databaseManager.Verify(x => x.ChangeEncryptionKey(newEncryptedKey), Times.Once);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void SetPassword_CallsFileManager_WhenAuthenticationMethodIsWindowsHello()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string fakeEncryptedKey = "EncryptedKey";
        _appSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;

        _encryptionManager.Setup(x => x.EncryptString(It.IsAny<string>(), testPassword)).Returns(fakeEncryptedKey);

        // Act
        _loginManager.SetPassword(testPassword);

        // Assert
        _fileManager.Verify(x => x.ProtectAndSavePassword(testPassword), Times.Once);
    }

    [Test]
    public void ChangePassword_CallsFileManager_WhenAuthenticationMethodIsWindowsHello()
    {
        // Arrange
        string oldPassword = "OldPassword123";
        string newPassword = "NewPassword123";
        string fakeEncryptedKey = "EncryptedKey";
        string decryptedKey = "DecryptedKey";
        string newEncryptedKey = "NewEncryptedKey";
        _appSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;

        _databaseManager.Setup(x => x.GetEncryptionKey()).Returns(new EncryptionKey { Key = fakeEncryptedKey });
        _encryptionManager.Setup(x => x.DecryptString(fakeEncryptedKey, oldPassword)).Returns(decryptedKey);
        _encryptionManager.Setup(x => x.EncryptString(decryptedKey, newPassword)).Returns(newEncryptedKey);

        // Act
        bool result = _loginManager.ChangePassword(newPassword, oldPassword);

        // Assert
        Assert.IsTrue(result);
        _fileManager.Verify(x => x.ProtectAndSavePassword(newPassword), Times.Once);
    }

}