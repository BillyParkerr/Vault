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

        // Act
        var result = _presenterManager.GetHomeViewPresenter(homeViewMock.Object);

        // Assert
        Assert.IsInstanceOf<HomeViewPresenter>(result);
    }

    [Test]
    public void TestGetLoginViewPresenter()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetLoginViewPresenter();

        // Assert
        Assert.IsInstanceOf<LoginViewPresenter>(result);
    }

    [Test]
    public void TestGetAuthenticationModeSelectionViewPresenter()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetAuthenticationModeSelectionViewPresenter();

        // Assert
        Assert.IsInstanceOf<AuthenticationModeSelectionViewPresenter>(result);
    }

    [Test]
    public void TestGetExportEncryptedFilePresenter()
    {
        // Arrange
        var encryptedFileToExport = new EncryptedFile();

        // Act
        var result = _presenterManager.GetExportEncryptedFilePresenter(encryptedFileToExport);

        // Assert
        Assert.IsInstanceOf<ExportEncryptedFilePresenter>(result);
    }

    [Test]
    public void TestGetImportEncryptedFilePresenter()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetImportEncryptedFilePresenter();

        // Assert
        Assert.IsInstanceOf<ImportEncryptedFilePresenter>(result);
    }

    [Test]
    public void TestGetRegistrationViewPresenter()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetRegistrationViewPresenter();

        // Assert
        Assert.IsInstanceOf<RegistrationViewPresenter>(result);
    }

    [Test]
    public void TestGetSettingsViewPresenter()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetSettingsViewPresenter();

        // Assert
        Assert.IsInstanceOf<SettingsViewPresenter>(result);
    }

    [Test]
    public void TestGetChangePasswordViewManager()
    {
        // Arrange
        // Act
        var result = _presenterManager.GetChangePasswordViewManager();

        // Assert
        Assert.IsInstanceOf<ChangePasswordViewPresenter>(result);
    }
}