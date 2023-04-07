using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

[TestFixture]
public class ChangePasswordViewPresenterTests
{
    private Mock<ILoginManager> _loginManagerMock;
    private Mock<IChangePasswordView> _viewMock;
    private ChangePasswordViewPresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _loginManagerMock = new Mock<ILoginManager>();
        _viewMock = new Mock<IChangePasswordView>();
        _presenter = new ChangePasswordViewPresenter(_loginManagerMock.Object, _viewMock.Object);
    }

    [Test]
    public void ConfirmPasswordEventHandler_ShowsBlankOldPasswordError()
    {
        // Arrange
        _viewMock.Setup(v => v.GivenOldPassword).Returns(string.Empty);

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowBlankOldPasswordError(), Times.Once);
    }

    [Test]
    public void ConfirmPasswordEventHandler_ShowsIncorrectOldPasswordError()
    {
        // Arrange
        _viewMock.Setup(v => v.GivenOldPassword).Returns("incorrect");
        _loginManagerMock.Setup(m => m.VerifyPassword("incorrect")).Returns(false);

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowIncorrectOldPasswordError(), Times.Once);
    }

    [Test]
    public void ConfirmPasswordEventHandler_ShowsBlankNewPasswordError()
    {
        // Arrange
        _viewMock.Setup(v => v.GivenOldPassword).Returns("correct");
        _loginManagerMock.Setup(m => m.VerifyPassword("correct")).Returns(true);
        _viewMock.Setup(v => v.GivenNewPassword).Returns(string.Empty);
        _viewMock.Setup(v => v.GivenSecondNewPassword).Returns("newpassword");

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowBlankNewPasswordError(), Times.Once);
    }

    [Test]
    public void ConfirmPasswordEventHandler_ShowsPasswordMismatchError()
    {
        // Arrange
        _viewMock.Setup(v => v.GivenOldPassword).Returns("correct");
        _loginManagerMock.Setup(m => m.VerifyPassword("correct")).Returns(true);
        _viewMock.Setup(v => v.GivenNewPassword).Returns("newpassword");
        _viewMock.Setup(v => v.GivenSecondNewPassword).Returns("different");

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowPasswordMismatchError(), Times.Once);
    }

    [Test]
    public void ConfirmPasswordEventHandler_ShowsNewPasswordTooShortError()
    {
        // Arrange
        _viewMock.Setup(v => v.GivenOldPassword).Returns("correct");
        _loginManagerMock.Setup(m => m.VerifyPassword("correct")).Returns(true);
        _viewMock.Setup(v => v.GivenNewPassword).Returns("short");
        _viewMock.Setup(v => v.GivenSecondNewPassword).Returns("short");

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        _viewMock.Verify(v => v.ShowNewPasswordTooShortError(), Times.Once);
    }

    [Test]
    public void ConfirmPasswordEventHandler_NewPasswordChosenEventRaised()
    {
        // Arrange
        string oldPassword = "correct";
        string newPassword = "newpassword";
        _viewMock.Setup(v => v.GivenOldPassword).Returns(oldPassword);
        _loginManagerMock.Setup(m => m.VerifyPassword(oldPassword)).Returns(true);
        _viewMock.Setup(v => v.GivenNewPassword).Returns(newPassword);
        _viewMock.Setup(v => v.GivenSecondNewPassword).Returns(newPassword);
        var newPasswordChosenEventRaised = false;

        _presenter.NewPasswordChosen += (_, e) =>
        {
            newPasswordChosenEventRaised = true;
            Assert.AreEqual(oldPassword, e.EnteredOldPassword);
            Assert.AreEqual(newPassword, e.EnteredNewPassword);
        };

        // Act
        _viewMock.Raise(v => v.ConfirmPasswordEvent += null, EventArgs.Empty);

        // Assert
        Assert.IsTrue(newPasswordChosenEventRaised);
    }
}