using Application.Managers;
using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

[TestFixture]
public class ExportEncryptedFilePresenterTests
{
    private Mock<IExportEncryptedFileView> _viewMock;
    private Mock<IFileManager> _fileManagerMock;
    private ExportEncryptedFilePresenter _presenter;
    private EncryptedFile _sampleEncryptedFile;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<IExportEncryptedFileView>();
        _fileManagerMock = new Mock<IFileManager>();
        _sampleEncryptedFile = new EncryptedFile { FilePath = "samplePath" };
        _presenter = new ExportEncryptedFilePresenter(_viewMock.Object, _fileManagerMock.Object, _sampleEncryptedFile);
    }

    [Test]
    public void Constructor_ShowsView()
    {
        _viewMock.Verify(v => v.Show(), Times.Once);
    }

    [Test]
    public void ConfirmEventHandler_ValidPassword_ExportsEncryptedFileAndClosesView()
    {
        // Arrange
        string password = "validPass";
        string selectedPath = "selectedPath";
        _viewMock.Setup(v => v.GivenPassword).Returns(password);
        _fileManagerMock.Setup(m => m.GetFolderPathFromExplorer()).Returns(selectedPath);
        _fileManagerMock.Setup(m => m.DownloadEncryptedFileFromVault(_sampleEncryptedFile.FilePath, selectedPath, password)).Returns(true);

        // Act
        _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

        // Assert
        _fileManagerMock.Verify(m => m.DownloadEncryptedFileFromVault(_sampleEncryptedFile.FilePath, selectedPath, password), Times.Once);
        _viewMock.Verify(v => v.Close(), Times.Once);
        _fileManagerMock.Verify(_ => _.OpenFolderInExplorer(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void ConfirmEventHandler_NoPassword_ShowsBlankPasswordError()
    {
        // Arrange
        string password = "";
        _viewMock.Setup(v => v.GivenPassword).Returns(password);

        // Act
        _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowBlankPasswordError(), Times.Once);
    }

    [Test]
    public void ConfirmEventHandler_PasswordTooShort_ShowsPasswordTooShortError()
    {
        // Arrange
        string password = "short";
        _viewMock.Setup(v => v.GivenPassword).Returns(password);

        // Act
        _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowPasswordTooShortError(), Times.Once);
    }
}