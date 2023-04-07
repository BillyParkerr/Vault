using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

public class RegistrationViewPresenterTests
{
    private Mock<ILoginManager> _loginManagerMock;
    private Mock<IRegisterView> _registerViewMock;
    private RegistrationViewPresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _loginManagerMock = new Mock<ILoginManager>();
        _registerViewMock = new Mock<IRegisterView>();

        _presenter = new RegistrationViewPresenter(_loginManagerMock.Object, _registerViewMock.Object);
    }

    [Test]
    public void RegisterEventHandler_ValidPasswords_RegistersUserAndClosesView()
    {
        _registerViewMock.SetupGet(v => v.GivenPassword).Returns("valid_password");
        _registerViewMock.SetupGet(v => v.GivenSecondPassword).Returns("valid_password");

        _registerViewMock.Raise(v => v.RegisterEvent += null, EventArgs.Empty);

        _loginManagerMock.Verify(lm => lm.SetPassword("valid_password"), Times.Once);
        _registerViewMock.Verify(v => v.Close(), Times.Once);
        Assert.IsTrue(_presenter.UserSuccessfullyRegistered);
    }

    [TestCase("", "")]
    [TestCase("password", "")]
    [TestCase("", "password")]
    public void RegisterEventHandler_BlankPassword_ShowsBlankPasswordError(string password1, string password2)
    {
        _registerViewMock.SetupGet(v => v.GivenPassword).Returns(password1);
        _registerViewMock.SetupGet(v => v.GivenSecondPassword).Returns(password2);

        _registerViewMock.Raise(v => v.RegisterEvent += null, EventArgs.Empty);

        _registerViewMock.Verify(v => v.ShowBlankPasswordError(), Times.Once);
        _loginManagerMock.Verify(lm => lm.SetPassword(It.IsAny<string>()), Times.Never);
        _registerViewMock.Verify(v => v.Close(), Times.Never);
        Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
    }

    [Test]
    public void RegisterEventHandler_NonMatchingPasswords_ShowsPasswordMismatchError()
    {
        _registerViewMock.SetupGet(v => v.GivenPassword).Returns("password1");
        _registerViewMock.SetupGet(v => v.GivenSecondPassword).Returns("password2");

        _registerViewMock.Raise(v => v.RegisterEvent += null, EventArgs.Empty);

        _registerViewMock.Verify(v => v.ShowPasswordMismatchError(), Times.Once);
        _loginManagerMock.Verify(lm => lm.SetPassword(It.IsAny<string>()), Times.Never);
        _registerViewMock.Verify(v => v.Close(), Times.Never);
        Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
    }

    [Test]
    public void RegisterEventHandler_PasswordTooShort_ShowsPasswordTooShortError()
    {
        _registerViewMock.SetupGet(v => v.GivenPassword).Returns("short");
        _registerViewMock.SetupGet(v => v.GivenSecondPassword).Returns("short");

        _registerViewMock.Raise(v => v.RegisterEvent += null, EventArgs.Empty);

        _registerViewMock.Verify(v => v.ShowPasswordTooShortError(), Times.Once);
        _loginManagerMock.Verify(lm => lm.SetPassword(It.IsAny<string>()), Times.Never);
        _registerViewMock.Verify(v => v.Close(), Times.Never);
        Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
    }
}