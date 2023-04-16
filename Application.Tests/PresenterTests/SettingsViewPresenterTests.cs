using System.Reflection;
using Application.Enums;
using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

[TestFixture]
public class SettingsViewPresenterTests
{
    private Mock<ISettingsView> _viewMock;
    private Mock<IFileManager> _fileManagerMock;
    private Mock<IWindowsHelloManager> _windowsHelloManagerMock;
    private Mock<IPresenterManager> _presenterManagerMock;
    private Mock<ILoginManager> _loginManagerMock;
    private AppSettings _appSettings;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<ISettingsView>();
        _fileManagerMock = new Mock<IFileManager>();
        _windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
        _presenterManagerMock = new Mock<IPresenterManager>();
        _loginManagerMock = new Mock<ILoginManager>();

        _appSettings = new AppSettings
        {
            AuthenticationMethod = AuthenticationMethod.Password,
            DeleteUnencryptedFileUponUpload = false,
            Mode = ApplicationMode.Basic,
            DefaultDownloadLocation = "DefaultPath"
        };
    }

    [Test]
    public async Task SettingsViewPresenter_SetupViewBasedUponAppSettings_DisablesButtonsBasedOnAppSettings()
    {
        // Arrange
        _windowsHelloManagerMock.Setup(x => x.IsWindowsHelloAvailable()).ReturnsAsync(true);

        // Act
        var presenter = new SettingsViewPresenter(
            _viewMock.Object, _fileManagerMock.Object, _appSettings,
            _windowsHelloManagerMock.Object, _presenterManagerMock.Object, _loginManagerMock.Object);

        await Task.Delay(50); // Allow time for async call to SetupViewBasedUponAppSettings

        // Assert
        _viewMock.Verify(x => x.DisablePasswordModeButton(), Times.Once);
        _viewMock.Verify(x => x.DisableBasicModeButton(), Times.Once);
    }

    [Test]
    public async Task SettingsViewPresenter_ToggleUserModeEventHandler_TogglesAppMode()
    {
        // Arrange
        _windowsHelloManagerMock.Setup(x => x.IsWindowsHelloAvailable()).ReturnsAsync(true);
        var presenter = new SettingsViewPresenter(
            _viewMock.Object, _fileManagerMock.Object, _appSettings,
            _windowsHelloManagerMock.Object, _presenterManagerMock.Object, _loginManagerMock.Object);

        await Task.Delay(50); // Allow time for async call to SetupViewBasedUponAppSettings

        // Act
        _viewMock.Raise(x => x.ToggleUserModeEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(x => x.EnableBasicModeButton(), Times.Once);
        _viewMock.Verify(x => x.DisableAdvancedModeButton(), Times.Once);
    }

    [Test]
    public async Task SettingsViewPresenter_ToggleAuthenticationModeEventHandler_TogglesAuthenticationMode()
    {
        // Arrange
        _windowsHelloManagerMock.Setup(x => x.IsWindowsHelloAvailable()).ReturnsAsync(true);
        var presenter = new SettingsViewPresenter(
            _viewMock.Object, _fileManagerMock.Object, _appSettings,
            _windowsHelloManagerMock.Object, _presenterManagerMock.Object, _loginManagerMock.Object);

        await Task.Delay(50); // Allow time for async call to SetupViewBasedUponAppSettings

        // Act
        _viewMock.Raise(x => x.ToggleAuthenticationModeEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(x => x.EnablePasswordModeButton(), Times.Once);
        _viewMock.Verify(x => x.DisableWindowsHelloModeButton(), Times.Once);
    }

    [Test]
    public async Task SettingsViewPresenter_ToggleAutomaticDeletionOfUploadedFilesEventHandler_TogglesDeletionSetting()
    {
        // Arrange
        _windowsHelloManagerMock.Setup(x => x.IsWindowsHelloAvailable()).ReturnsAsync(true);
        var presenter = new SettingsViewPresenter(
            _viewMock.Object, _fileManagerMock.Object, _appSettings,
            _windowsHelloManagerMock.Object, _presenterManagerMock.Object, _loginManagerMock.Object);

        await Task.Delay(50); // Allow time for async call to SetupViewBasedUponAppSettings

        // Act
        _viewMock.Raise(x => x.ToggleAutomaticDeletionOfUploadedFilesEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(x => x.SetDeletionOfUploadedFilesToYes(), Times.Once);
    }

    [Test]
    public void SettingsViewPresenter_ChangeDefaultDownloadLocationEventHandler_ChangesDownloadLocation()
    {
        // Arrange
        _windowsHelloManagerMock.Setup(x => x.IsWindowsHelloAvailable()).ReturnsAsync(true);
        _fileManagerMock.Setup(x => x.GetFolderPathFromExplorer(It.IsAny<string>())).Returns("NewPath");
        var presenter = new SettingsViewPresenter(
            _viewMock.Object, _fileManagerMock.Object, _appSettings,
            _windowsHelloManagerMock.Object, _presenterManagerMock.Object, _loginManagerMock.Object);

        // Act
        _viewMock.Raise(x => x.ChangeDefaultDownloadLocationEvent += null, EventArgs.Empty);

        // Assert
        var uncommittedAppSettings = presenter.GetType().GetField("_uncommitedAppSettings", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(presenter) as AppSettings;
        Assert.AreEqual("NewPath", uncommittedAppSettings.DefaultDownloadLocation);
    }
}