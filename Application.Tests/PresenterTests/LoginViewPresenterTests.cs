using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

public class LoginViewPresenterTests
{
    private Mock<ILoginView> _viewMock;
    private Mock<ILoginManager> _loginManagerMock;
    private Mock<IEncryptionManager> _encryptionManagerMock;
    private LoginViewPresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<ILoginView>();
        _loginManagerMock = new Mock<ILoginManager>();
        _encryptionManagerMock = new Mock<IEncryptionManager>();

        _presenter = new LoginViewPresenter(_viewMock.Object, _loginManagerMock.Object, _encryptionManagerMock.Object);
    }

    [Test]
    public void LoginEventHandler_ValidPassword_AuthenticatesUserAndClosesView()
    {
        _viewMock.SetupGet(v => v.GivenPassword).Returns("valid_password");
        _loginManagerMock.Setup(lm => lm.VerifyPassword("valid_password")).Returns(true);

        _viewMock.Raise(v => v.LoginEvent += null, EventArgs.Empty);

        _loginManagerMock.Verify(lm => lm.VerifyPassword("valid_password"), Times.Once);
        _encryptionManagerMock.Verify(em => em.SetEncryptionPassword("valid_password"), Times.Once);
        _viewMock.Verify(v => v.Close(), Times.Once);
        Assert.IsTrue(_presenter.UserSuccessfullyAuthenticated);
    }

    [Test]
    public void LoginEventHandler_BlankPassword_ShowsBlankPasswordError()
    {
        _viewMock.SetupGet(v => v.GivenPassword).Returns(string.Empty);

        _viewMock.Raise(v => v.LoginEvent += null, EventArgs.Empty);

        _viewMock.Verify(v => v.ShowBlankPasswordGivenError(), Times.Once);
        _loginManagerMock.Verify(lm => lm.VerifyPassword(It.IsAny<string>()), Times.Never);
        _encryptionManagerMock.Verify(em => em.SetEncryptionPassword(It.IsAny<string>()), Times.Never);
        _viewMock.Verify(v => v.Close(), Times.Never);
        Assert.IsFalse(_presenter.UserSuccessfullyAuthenticated);
    }

    [Test]
    public void LoginEventHandler_InvalidPassword_ShowsIncorrectPasswordError()
    {
        _viewMock.SetupGet(v => v.GivenPassword).Returns("invalid_password");
        _loginManagerMock.Setup(lm => lm.VerifyPassword("invalid_password")).Returns(false);

        _viewMock.Raise(v => v.LoginEvent += null, EventArgs.Empty);

        _viewMock.Verify(v => v.ShowIncorrectPasswordError(), Times.Once);
        _loginManagerMock.Verify(lm => lm.VerifyPassword("invalid_password"), Times.Once);
        _encryptionManagerMock.Verify(em => em.SetEncryptionPassword(It.IsAny<string>()), Times.Never);
        _viewMock.Verify(v => v.Close(), Times.Never);
        Assert.IsFalse(_presenter.UserSuccessfullyAuthenticated);
    }
}