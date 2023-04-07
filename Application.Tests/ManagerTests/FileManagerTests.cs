using Application.Managers;
using Application.Models;
using Moq;

namespace Application.Tests.ManagerTests;

[TestFixture]
public class FileManagerTests
{
    private Mock<IEncryptionManager> _encryptionManager;
    private Mock<IDatabaseManager> _databaseManager;
    private Mock<IFileMonitoringManager> _fileMonitoringManager;
    private AppSettings _appSettings;
    private FileManager _fileManager;
    private readonly TestHelper _testHelper = new();
    private List<string> filesToDelete;

    [SetUp]
    public void Setup()
    {
        filesToDelete = new List<string>();

        _encryptionManager = new Mock<IEncryptionManager>();
        _databaseManager = new Mock<IDatabaseManager>();
        _fileMonitoringManager = new Mock<IFileMonitoringManager>();
        _appSettings = new AppSettings();
        _fileManager = new FileManager(_encryptionManager.Object, _databaseManager.Object,
            _fileMonitoringManager.Object, _appSettings);
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
    public void GetAllFilesInVault_ReturnsListOfEncryptedFiles()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        var secondTestFile = _testHelper.CreateTextFile();

        filesToDelete.Add(testFile);
        filesToDelete.Add(secondTestFile);

        var encryptedFiles = new List<EncryptedFile>
        {
            new EncryptedFile { FilePath = testFile, UniquePassword = false},
            new EncryptedFile { FilePath = secondTestFile, UniquePassword = false}
        };

        _databaseManager.Setup(x => x.GetAllEncryptedFiles()).Returns(encryptedFiles);
        _encryptionManager.Setup(x => x.DecryptString(testFile, It.IsAny<string>()))
            .Returns(Path.GetFileName(testFile));
        _encryptionManager.Setup(x => x.DecryptString(secondTestFile, It.IsAny<string>()))
            .Returns(Path.GetFileName(secondTestFile));

        // Act
        var result = _fileManager.GetAllFilesInVault();

        // Assert
        Assert.AreEqual(encryptedFiles.Count, result.Count);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void AddFileToVault_ReturnsTrue_WhenFileIsSuccessfullyAdded()
    {
        // Arrange
        string filePath = "file.txt";
        _databaseManager.Setup(x => x.AddEncryptedFile(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();

        // Act
        bool result = _fileManager.AddFileToVault(filePath);

        // Assert
        Assert.IsTrue(result);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void DownloadFileFromVault_ReturnsTrueIfFileExists()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        filesToDelete.Add(testFile);

        // Mock the EncryptionManager.DecryptFile method to return the decrypted file path
        _encryptionManager.Setup(x => x.DecryptFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(testFile);

        // Act
        var result = _fileManager.DownloadFileFromVault("TESTPATH", "DESTIONATIONTESTPATH");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void DeleteFileFromVault_DatabaseManagerCalledAndReturnsTrue()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        filesToDelete.Add(testFile);

        // Act
        var result = _fileManager.DeleteFileFromVault(testFile);

        // Assert
        Assert.IsTrue(result);
        _databaseManager.Verify(x => x.DeleteEncryptedFileByFilePath(testFile), Times.Once);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void OpenFileFromVaultAndReencryptUponClosure_ReturnsTrueIfFileExists()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        filesToDelete.Add(testFile);

        // Copy the test file to the decrypted files temp directory
        var copiedFilePath = Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, Path.GetFileName(testFile));
        File.Copy(testFile, copiedFilePath);
        filesToDelete.Add(copiedFilePath);

        // Mock the EncryptionManager.DecryptFile method to return the decrypted file path
        _encryptionManager.Setup(x => x.DecryptFile(testFile, DirectoryPaths.DecryptedFilesTempDirectory, It.IsAny<string>()))
            .Returns(testFile);

        // Mock the EncryptionManager.DecryptString method to return the file name
        _encryptionManager.Setup(x => x.DecryptString(Path.GetFileNameWithoutExtension(testFile), It.IsAny<string>()))
            .Returns(Path.GetFileName(testFile));

        // Act
        var result = _fileManager.OpenFileFromVaultAndReencryptUponClosure(testFile);

        // Assert
        Assert.IsTrue(result);
        _fileMonitoringManager.Verify(x => x.Initilise(copiedFilePath, testFile), Times.Once);
    }

    [Test]
    public void DownloadEncryptedFileFromVault_Success()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        filesToDelete.Add(testFile);

        // Copy the test file to the decrypted files temp directory
        var copiedFilePath = Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, Path.GetFileName(testFile));
        File.Copy(testFile, copiedFilePath);

        string newEncryptionPassword = "TEST PASSWORD";
        string destionationFilePath = DirectoryPaths.EncryptedFilesCustomDirectory;

        // Mock the EncryptionManager.DecryptString method to return the file name
        _encryptionManager.Setup(x => x.DecryptString(Path.GetFileNameWithoutExtension(testFile), It.IsAny<string>()))
            .Returns(Path.GetFileName(testFile));

        _encryptionManager.Setup(x => x.EncryptFile(copiedFilePath, newEncryptionPassword)).Returns(copiedFilePath);

        filesToDelete.Add(Path.Combine(destionationFilePath, Path.GetFileName(copiedFilePath)));

        // Act
        var result = _fileManager.DownloadEncryptedFileFromVault(testFile, destionationFilePath, newEncryptionPassword);

        Assert.IsTrue(result);
    }

    [Test]
    public void ImportEncryptedFileToVault_Success()
    {
        // Arrange
        var testFile = _testHelper.CreateTextFile();
        filesToDelete.Add(testFile);

        // Copy the test file to the decrypted files temp directory
        var copiedFilePath = Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, Path.GetFileName(testFile));
        File.Copy(testFile, copiedFilePath);

        string encryptionPassword = "TEST PASSWORD";

        // Mock the EncryptionManager.DecryptString method to return the file name
        _encryptionManager.Setup(x => x.DecryptString(Path.GetFileNameWithoutExtension(testFile), encryptionPassword))
            .Returns(Path.GetFileName(testFile));

        _encryptionManager.Setup(x => x.DecryptFile(testFile, DirectoryPaths.DecryptedFilesTempDirectory, It.IsAny<string>())).Returns(copiedFilePath);

        // Mock the DatabaseManager to not throw an exception when adding an encrypted file
        _databaseManager.Setup(x => x.AddEncryptedFile(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
        _databaseManager.Setup(x => x.SaveChanges()).Verifiable();

        filesToDelete.Add(Path.Combine(DirectoryPaths.DecryptedFilesTempDirectory, Path.GetFileName(copiedFilePath)));

        // Act
        var result = _fileManager.ImportEncryptedFileToVault(testFile, encryptionPassword);

        // Assert
        Assert.IsTrue(result);
        _databaseManager.Verify(x => x.AddEncryptedFile(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _databaseManager.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public void ReadAndReturnProtectedPassword_ReturnsDecryptedPassword()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string tempEncryptedKeyPath = Path.Combine(Path.GetTempPath(), "TempEncryptedKey.bin");
        filesToDelete.Add(tempEncryptedKeyPath);

        // Save the encrypted password to the temp file
        _fileManager.ProtectAndSavePassword(testPassword);

        // Act
        string decryptedPassword = _fileManager.ReadAndReturnProtectedPassword();

        // Assert
        Assert.AreEqual(testPassword, decryptedPassword);
    }

    [Test]
    public void ProtectedPasswordExists_ReturnsTrue_WhenFileExists()
    {
        // Arrange
        string testPassword = "TestPassword123";
        string tempEncryptedKeyPath = Path.Combine(Path.GetTempPath(), "TempEncryptedKey.bin");
        filesToDelete.Add(tempEncryptedKeyPath);

        // Save the encrypted password to the temp file
        _fileManager.ProtectAndSavePassword(testPassword);

        // Act
        bool result = _fileManager.ProtectedPasswordExists();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void ProtectAndSavePassword_SavesEncryptedPasswordToFile()
    {
        // Arrange
        string testPassword = "TestPassword123";
        filesToDelete.Add(DirectoryPaths.EncryptedKeyPath);

        // Act
        _fileManager.ProtectAndSavePassword(testPassword);

        // Assert
        Assert.IsTrue(File.Exists(DirectoryPaths.EncryptedKeyPath));
    }

    [Test]
    public void ProtectedPasswordExists_ReturnsFalse_WhenFileDoesNotExist()
    {
        // Ensure the file does not exist
        if (File.Exists(DirectoryPaths.EncryptedKeyPath))
        {
            File.Delete(DirectoryPaths.EncryptedKeyPath);
        }

        // Act
        bool result = _fileManager.ProtectedPasswordExists();

        // Assert
        Assert.IsFalse(result);
    }
}