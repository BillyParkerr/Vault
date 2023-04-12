using Application.Managers;
using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.ManagerTests;

public class PresenterManagerTests
{
    private Mock<IDatabaseManager> _databaseManagerMock;
    private Mock<IFileManager> _fileManagerMock;
    private Mock<ILoginManager> _loginManagerMock;
    private Mock<IWindowsHelloManager> _windowsHelloManagerMock;
    private AppSettings _appSettings;
    private PresenterManager _presenterManager;

    [SetUp]
    public void Setup()
    {
        _databaseManagerMock = new Mock<IDatabaseManager>();
        _fileManagerMock = new Mock<IFileManager>();
        _loginManagerMock = new Mock<ILoginManager>();
        _windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
        _appSettings = new AppSettings();

        _presenterManager = new PresenterManager(
            _databaseManagerMock.Object,
            _fileManagerMock.Object,
            _loginManagerMock.Object,
            _windowsHelloManagerMock.Object,
            _appSettings
        );
    }

    [Test]
    public void GetHomeViewPresenter_ReturnsHomeViewPresenter()
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
            new EncryptedFile { FilePath = "path1", DecryptedFileInformation = fileInformation},
            new EncryptedFile { FilePath = "path2", DecryptedFileInformation = fileInformation}
        };

        _fileManagerMock.Setup(x => x.GetAllFilesInVault()).Returns(encryptedFiles);

        // Act
        var result = _presenterManager.GetHomeViewPresenter(homeViewMock.Object);

        // Assert
        Assert.IsInstanceOf<HomeViewPresenter>(result);
    }

    [Test]
    public void GetExportEncryptedFilePresenter_ReturnsExportEncryptedFilePresenter()
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
    public void GetImportEncryptedFilePresenter_ReturnsImportEncryptedFilePresenter()
    {
        // Arrange
        var importEncryptedFileViewMock = new Mock<IImportEncryptedFileView>();

        // Act
        var result = _presenterManager.GetImportEncryptedFilePresenter(importEncryptedFileViewMock.Object);

        // Assert
        Assert.IsInstanceOf<ImportEncryptedFilePresenter>(result);
    }

    [Test]
    public void GetSettingsViewPresenter_ReturnsSettingsViewPresenter()
    {
        // Arrange
        var settingsViewMock = new Mock<ISettingsView>();

        // Act
        var result = _presenterManager.GetSettingsViewPresenter(settingsViewMock.Object);

        // Assert
        Assert.IsInstanceOf<SettingsViewPresenter>(result);
    }

    [Test]
    public void GetChangePasswordViewPresenter_ReturnsChangePasswordViewPresenter()
    {
        // Arrange
        var changePasswordViewMock = new Mock<IChangePasswordView>();

        // Act
        var result = _presenterManager.GetChangePasswordViewManager(changePasswordViewMock.Object);

        // Assert
        Assert.IsInstanceOf<ChangePasswordViewPresenter>(result);
    }

    [Test]
    public void GetWindowsHelloRegisterViewPresenter_ReturnsWindowsHelloRegisterView()
    {
        // Arrange
        var windowsHelloRegisterViewMock = new Mock<IWindowsHelloRegisterView>();

        // Act
        var result = _presenterManager.GetWindowsHelloRegisterViewPresenter(windowsHelloRegisterViewMock.Object);

        // Assert
        Assert.IsInstanceOf<WindowsHelloRegisterViewPresenter>(result);
    }
}