using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

public class ImportEncryptedFilePresenterTests
{
    private Mock<IImportEncryptedFileView> _viewMock;
    private Mock<IFileManager> _fileManagerMock;
    private ImportEncryptedFilePresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<IImportEncryptedFileView>();
        _fileManagerMock = new Mock<IFileManager>();
        _fileManagerMock.Setup(fm => fm.GetFilePathFromExplorer(It.IsAny<string>())).Returns("test.aes");

        _presenter = new ImportEncryptedFilePresenter(_viewMock.Object, _fileManagerMock.Object);
    }

    [Test]
    public void ConfirmEventHandler_ValidPassword_ImportsFileAndClosesView()
    {
        _viewMock.SetupGet(v => v.GivenPassword).Returns("valid_password");
        _fileManagerMock.Setup(fm => fm.ImportEncryptedFileToVault("test.aes", "valid_password")).Returns(true);

        _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

        _fileManagerMock.Verify(fm => fm.ImportEncryptedFileToVault("test.aes", "valid_password"), Times.Once);
        _viewMock.Verify(v => v.Close(), Times.Once);
    }

    [Test]
    public void ConfirmEventHandler_BlankPassword_ShowsBlankPasswordError()
    {
        _viewMock.SetupGet(v => v.GivenPassword).Returns(string.Empty);

        _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

        _viewMock.Verify(v => v.ShowBlankPasswordError(), Times.Once);
        _fileManagerMock.Verify(fm => fm.ImportEncryptedFileToVault(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _viewMock.Verify(v => v.Close(), Times.Never);
    }
}