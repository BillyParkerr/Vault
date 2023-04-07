using Application.Managers;
using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.ManagerTests;

public class PresenterManagerTests
{
    private Mock<IDatabaseManager> _databaseManagerMock;
    private Mock<IEncryptionManager> _encryptionManagerMock;
    private Mock<IFileManager> _fileManagerMock;
    private Mock<ILoginManager> _loginManagerMock;
    private Mock<IWindowsHelloManager> _windowsHelloManagerMock;
    private AppSettings _appSettings;
    private PresenterManager _presenterManager;

    [SetUp]
    public void Setup()
    {
        _databaseManagerMock = new Mock<IDatabaseManager>();
        _encryptionManagerMock = new Mock<IEncryptionManager>();
        _fileManagerMock = new Mock<IFileManager>();
        _loginManagerMock = new Mock<ILoginManager>();
        _windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
        _appSettings = new AppSettings();

        _presenterManager = new PresenterManager(
            _databaseManagerMock.Object,
            _encryptionManagerMock.Object,
            _fileManagerMock.Object,
            _loginManagerMock.Object,
            _windowsHelloManagerMock.Object,
            _appSettings
        );
    }

    [Test]
    public void TestGetHomeViewPresenter()
    {
        // Arrange
        var homeViewMock = new Mock<IHomeView>();
        var fileInformation = new FileInformation
        {
            FileExtension = ".txt",
            FileName = "TEST",
            FileSize = "20Mb"
        };

        var encryptedFiles = new List<EncryptedFile>
        {
            new EncryptedFile { FilePath = "path1", UniquePassword = false, DecryptedFileInformation = fileInformation},
            new EncryptedFile { FilePath = "path2", UniquePassword = false, DecryptedFileInformation = fileInformation}
        };

        _fileManagerMock.Setup(x => x.GetAllFilesInVault()).Returns(encryptedFiles);

        // Act
        var result = _presenterManager.GetHomeViewPresenter(homeViewMock.Object);

        // Assert
        Assert.IsInstanceOf<HomeViewPresenter>(result);
    }

    [Test]
    public void TestGetLoginViewPresenter()
    {
        // Arrange
        var loginViewMock = new Mock<ILoginView>();

        // Act
        var result = _presenterManager.GetLoginViewPresenter(loginViewMock.Object);

        // Assert
        Assert.IsInstanceOf<LoginViewPresenter>(result);
    }

    [Test]
    public void TestGetAuthenticationModeSelectionViewPresenter()
    {
        // Arrange
        var authenticationModeSelectionViewMock = new Mock<IAuthenticationModeSelectionView>();

        // Act
        var result = _presenterManager.GetAuthenticationModeSelectionViewPresenter(authenticationModeSelectionViewMock.Object);

        // Assert
        Assert.IsInstanceOf<AuthenticationModeSelectionViewPresenter>(result);
    }

    [Test]
    public void TestGetExportEncryptedFilePresenter()
    {
        // Arrange
        var encryptedFileToExport = new EncryptedFile();
        var exportEncryptedFileViewMock = new Mock<IExportEncryptedFileView>();

        // Act
        var result = _presenterManager.GetExportEncryptedFilePresenter(encryptedFileToExport, exportEncryptedFileViewMock.Object);

        // Assert
        Assert.IsInstanceOf<ExportEncryptedFilePresenter>(result);
    }

    [Test]
    public void TestGetImportEncryptedFilePresenter()
    {
        // Arrange
        var importEncryptedFileViewMock = new Mock<IImportEncryptedFileView>();

        // Act
        var result = _presenterManager.GetImportEncryptedFilePresenter(importEncryptedFileViewMock.Object);

        // Assert
        Assert.IsInstanceOf<ImportEncryptedFilePresenter>(result);
    }

    [Test]
    public void TestGetRegistrationViewPresenter()
    {
        // Arrange
        var registerViewMock = new Mock<IRegisterView>();

        // Act
        var result = _presenterManager.GetRegistrationViewPresenter(registerViewMock.Object);

        // Assert
        Assert.IsInstanceOf<RegistrationViewPresenter>(result);
    }

    [Test]
    public void TestGetSettingsViewPresenter()
    {
        // Arrange
        var settingsViewMock = new Mock<ISettingsView>();

        // Act
        var result = _presenterManager.GetSettingsViewPresenter(settingsViewMock.Object);

        // Assert
        Assert.IsInstanceOf<SettingsViewPresenter>(result);
    }

    [Test]
    public void TestGetChangePasswordViewManager()
    {
        // Arrange
        var changePasswordViewMock = new Mock<IChangePasswordView>();

        // Act
        var result = _presenterManager.GetChangePasswordViewManager(changePasswordViewMock.Object);

        // Assert
        Assert.IsInstanceOf<ChangePasswordViewPresenter>(result);
    }
}