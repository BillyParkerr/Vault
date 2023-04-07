using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;
using System.ComponentModel;
using System.Windows.Forms;

namespace Application.Tests.PresenterTests;

[TestFixture]
public class HomeViewPresenterTests
{
    private Mock<IHomeView> _viewMock;
    private Mock<IFileManager> _fileManagerMock;
    private Mock<IDatabaseManager> _databaseManagerMock;
    private Mock<IPresenterManager> _presenterManagerMock;
    private AppSettings _appSettings;
    private HomeViewPresenter _presenter;
    private List<EncryptedFile> _filesInVault;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<IHomeView>();
        _fileManagerMock = new Mock<IFileManager>();
        _databaseManagerMock = new Mock<IDatabaseManager>();
        _presenterManagerMock = new Mock<IPresenterManager>();
        _appSettings = new AppSettings { Mode = ApplicationMode.Basic, AuthenticationMethod = AuthenticationMethod.Password};

        var fileInformation = new FileInformation
        {
            FileExtension = ".txt",
            FileName = "TEST",
            FileSize = "20Mb"
        };

        var fileInformation2 = new FileInformation
        {
            FileExtension = ".txt",
            FileName = "OTHER_FILE",
            FileSize = "10Mb"
        };

        _filesInVault = new List<EncryptedFile>
        {
            new EncryptedFile { FilePath = "path1", UniquePassword = false, DecryptedFileInformation = fileInformation},
            new EncryptedFile { FilePath = "path2", UniquePassword = false, DecryptedFileInformation = fileInformation2}
        };

        _fileManagerMock.Setup(x => x.GetAllFilesInVault()).Returns(_filesInVault);

        _presenter = new TestHomeViewPresenter(
            _viewMock.Object,
            _fileManagerMock.Object,
            _databaseManagerMock.Object,
            _presenterManagerMock.Object,
            _appSettings);
    }

    // This is used to avoid messages and views showing when running the tests. It only overrides a specific method so all all other functionality should be equal to HomeViewPresenter.
    // Overriding methods should be kept to a minimum to ensure tests provide value.
    public class TestHomeViewPresenter : HomeViewPresenter
    {
        public TestHomeViewPresenter(IHomeView view, IFileManager fileManager, IDatabaseManager databaseManager,
            IPresenterManager presenterManager, AppSettings appSettings)
            : base(view, fileManager, databaseManager, presenterManager, appSettings)
        {
        }

        protected override void ShowMessageBox(string message)
        {
            // Do nothiing
        }
    }

    [TestCase(ApplicationMode.Advanced)]
    [TestCase(ApplicationMode.Basic)]
    public void SetupViewBasedUponAppSettings(ApplicationMode appMode)
    {
        // Arrange
        _appSettings.Mode = appMode;

        // This is required to refresh the verify count
        _viewMock = new Mock<IHomeView>();

        // Act
        var _ = new HomeViewPresenter(_viewMock.Object,
            _fileManagerMock.Object,
            _databaseManagerMock.Object,
            _presenterManagerMock.Object,
            _appSettings);

        // Assert
        switch (appMode){
            case ApplicationMode.Advanced:
                _viewMock.Verify(_ => _.SetAdvancedModeView(), Times.Once);
                break;
            case ApplicationMode.Basic:
                _viewMock.Verify(_ => _.SetBasicModeView(), Times.Once);
                break;
        }
    }

    [Test]
    public void OpenSettingsEventHandler_PausesAndResumesViewAndConfiguresViewBasedUponAppSettings()
    {
        // Arrange
        var settingsViewMock = new Mock<ISettingsView>();
        settingsViewMock.Setup(x => x.Show()).Verifiable();
        var fileManagerMock = new Mock<IFileManager>();
        var appSettings = new AppSettings();
        var windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
        var presenterManagerMock = new Mock<IPresenterManager>();
        var loginManagerMock = new Mock<ILoginManager>();

        var settingsViewPresenter = new Mock<SettingsViewPresenter>(settingsViewMock.Object, fileManagerMock.Object, appSettings, windowsHelloManagerMock.Object, presenterManagerMock.Object, loginManagerMock.Object);
        _presenterManagerMock.Setup(x => x.GetSettingsViewPresenter(It.IsAny<ISettingsView>()))
            .Returns(settingsViewPresenter.Object);

        // Act
        // Raise the OpenSettingsEvent, this will cause OpenSettingsEventHandler method to be called.
        _viewMock.Raise(v => v.OpenSettingsEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(x => x.PauseView(), Times.Once);
        _viewMock.Verify(x => x.ResumeView(), Times.Never); // Not called yet

        // Raise the SettingsConfirmed Event
        settingsViewPresenter.Raise(s => s.SettingsConfirmed += null, EventArgs.Empty);

        _viewMock.Verify(x => x.ResumeView(), Times.Once);
        _viewMock.Verify(x => x.SetBasicModeView());
    }

    [Test]
    public void AddFileToVaultEventHandler_AddsFileWhenSelected()
    {
        // Arrange
        string testFilePath = "testfile.txt";
        _fileManagerMock.Setup(x => x.GetFilePathFromExplorer(It.IsAny<string>())).Returns(testFilePath);
        _fileManagerMock.Setup(x => x.AddFileToVault(testFilePath, It.IsAny<string>())).Returns(true);

        // Act
        _viewMock.Raise(_ => _.AddFileToVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(x => x.AddFileToVault(testFilePath, It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void AddFileToVaultEventHandler_DoesNotAddFileWhenNotSelected()
    {
        // Arrange
        _fileManagerMock.Setup(x => x.GetFilePathFromExplorer(It.IsAny<string>())).Returns((string)null);

        // Act
        _viewMock.Raise(_ => _.AddFileToVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(x => x.AddFileToVault(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void AddFolderToVaultEventHandler_AddsFolderWhenSelected()
    {
        // Arrange
        string testFolderPath = "testfolder";
        _fileManagerMock.Setup(x => x.GetFolderPathFromExplorer()).Returns(testFolderPath);
        _fileManagerMock.Setup(x => x.ZipFolderAndAddToVault(testFolderPath, It.IsAny<string>())).Returns(true);

        // Act
        _viewMock.Raise(_ => _.AddFolderToVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(x => x.ZipFolderAndAddToVault(testFolderPath, It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void AddFolderToVaultEventHandler_DoesNotAddFolderWhenNotSelected()
    {
        // Arrange
        _fileManagerMock.Setup(x => x.GetFolderPathFromExplorer()).Returns((string)null);

        // Act
        _viewMock.Raise(_ => _.AddFolderToVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(x => x.ZipFolderAndAddToVault(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void DownloadFileFromVaultEventHandler_DownloadsFileWhenFileAndFolderSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(_filesInVault.First().DecryptedFileInformation);

        var selectedPath = @"C:\\TEST";
        _fileManagerMock.Setup(_ => _.GetFolderPathFromExplorer()).Returns(selectedPath);
        _fileManagerMock.Setup(_ => _.DownloadFileFromVault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        // Act
        _viewMock.Raise(_ => _.DownloadFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DownloadFileFromVault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _fileManagerMock.Verify(_ => _.OpenFolderInExplorer(selectedPath), Times.Once);
    }

    [Test]
    public void DownloadFileFromVaultEventHandler_DoesNotDownloadFileWhenFileNotSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns((FileInformation)null);

        // Act
        _viewMock.Raise(_ => _.DownloadFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DownloadFileFromVault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _fileManagerMock.Verify(_ => _.OpenFolderInExplorer(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void DownloadFileFromVaultEventHandler_DoesNotDownloadFileWhenFolderNotSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(_filesInVault.First().DecryptedFileInformation);
        _fileManagerMock.Setup(_ => _.GetFolderPathFromExplorer()).Returns((string)null);

        // Act
        _viewMock.Raise(_ => _.DownloadFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DownloadFileFromVault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _fileManagerMock.Verify(_ => _.OpenFolderInExplorer(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void DownloadFileFromVaultEventHandler_DoesNotOpenFolderWhenDownloadFails()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(_filesInVault.First().DecryptedFileInformation);

        var selectedPath = @"C:\\TEST";
        _fileManagerMock.Setup(_ => _.GetFolderPathFromExplorer()).Returns(selectedPath);
        _fileManagerMock.Setup(_ => _.DownloadFileFromVault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        _viewMock.Raise(_ => _.DownloadFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.OpenFolderInExplorer(selectedPath), Times.Never);
    }

    [Test]
    public void OpenFileFromVaultEventHandler_OpensFileWhenFileSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(_filesInVault.First().DecryptedFileInformation);

        // Act
        _viewMock.Raise(_ => _.OpenFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.OpenFileFromVaultAndReencryptUponClosure(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void OpenFileFromVaultEventHandler_DoesNotOpensFileWhenFileNotSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns((FileInformation)null);

        // Act
        _viewMock.Raise(_ => _.OpenFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.OpenFileFromVaultAndReencryptUponClosure(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void DeleteFileFromVaultEventHandler_DeletesFileWhenFileSelected()
    {
        // Arrange
        var selectedFile = _filesInVault.First();
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(selectedFile.DecryptedFileInformation);
        _fileManagerMock.Setup(_ => _.DeleteFileFromVault(It.IsAny<string>())).Returns(true);

        // Act
        _viewMock.Raise(_ => _.DeleteFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DeleteFileFromVault(selectedFile.FilePath), Times.Once);
    }

    [Test]
    public void DeleteFileFromVaultEventHandler_DoesNotDeletesFileWhenFileNotSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns((FileInformation)null);

        // Act
        _viewMock.Raise(_ => _.DeleteFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DeleteFileFromVault(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void DeleteFileFromVaultEventHandler_ShowsViewErrorIfDownloadFails()
    {
        // Arrange
        var selectedFile = _filesInVault.First();
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(selectedFile.DecryptedFileInformation);
        _fileManagerMock.Setup(_ => _.DeleteFileFromVault(It.IsAny<string>())).Returns(false);

        // Act
        _viewMock.Raise(_ => _.DeleteFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(_ => _.DeleteFileFromVault(selectedFile.FilePath), Times.Once);
        _viewMock.Verify(_ => _.ShowFailedToDeleteError(), Times.Once);
    }

    [Test]
    public void ExportFileFromVaultEventHandler_GetsExportEncryptedFilePresenterWhenFileSelected()
    {
        // Arrange
        var selectedFile = _filesInVault.First();
        _viewMock.SetupGet(_ => _.SelectedFile).Returns(selectedFile.DecryptedFileInformation);

        // Act
        _viewMock.Raise(_ => _.ExportFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _presenterManagerMock.Verify(_ => _.GetExportEncryptedFilePresenter(It.IsAny<EncryptedFile>(), It.IsAny<IExportEncryptedFileView>()), Times.Once);
    }

    [Test]
    public void ExportFileFromVaultEventHandler_DoesNotGetsExportEncryptedFilePresenterWhenNoFileSelected()
    {
        // Arrange
        _viewMock.SetupGet(_ => _.SelectedFile).Returns((FileInformation)null);

        // Act
        _viewMock.Raise(_ => _.ExportFileFromVaultEvent += null, EventArgs.Empty);

        // Assert
        _presenterManagerMock.Verify(_ => _.GetExportEncryptedFilePresenter(It.IsAny<EncryptedFile>(), It.IsAny<IExportEncryptedFileView>()), Times.Never);
    }

    [Test]
    public void ImportFileToVaultEventHandler_EventInvokedGetsImportEncryptedFilePresenter()
    {
        // Act
        _viewMock.Raise(_ => _.ImportFileToVaultEvent += null, EventArgs.Empty);

        // Assert
        _presenterManagerMock.Verify(_ => _.GetImportEncryptedFilePresenter(It.IsAny<IImportEncryptedFileView>()));
    }

    [TestCase("")]
    [TestCase("       ")]
    [TestCase((string)null)]
    [TestCase("TEST VALUE")]
    public void SearchFilterAppliedEventHandler_WhenSearchValueNotGivenLoadAllFilesInVault(string searchValue)
    {
        // Arrange
        _fileManagerMock.Setup(x => x.GetAllFilesInVault()).Returns(_filesInVault);
        _viewMock.SetupGet(x => x.SearchValue).Returns(searchValue);

        // Act
        _viewMock.Raise(_ => _.SearchFilterAppliedEvent += null, EventArgs.Empty);

        // Assert

        // The minimum times GetAllFilesInVault will be called in one as it is called in the HomeViewPresenter constructor.
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            _fileManagerMock.Verify(_ => _.GetAllFilesInVault(), Times.Exactly(2));
        }
        else
        {
            // Ideally we would assert against the binding source being updated here.
            // However due to the field being private this is not possible.
            _fileManagerMock.Verify(_ => _.GetAllFilesInVault(), Times.Once);
        }
    }

    [Test]
    public void FormClosingEventHandler_CleansUpTempFiles()
    {
        // Arrange
        var cancelEventArgs = new CancelEventArgs();
        var formClosingEventArgs = new FormClosingEventArgs(CloseReason.UserClosing, cancelEventArgs.Cancel);

        // Act
        _viewMock.Raise(_ => _.FormClosingEvent += null, formClosingEventArgs);

        // Assert
        _fileManagerMock.Verify(_ => _.CleanupTempFiles(), Times.Once);
    }
}