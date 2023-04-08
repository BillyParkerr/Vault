using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests
{
    [TestFixture]
    public class WindowsHelloRegisterViewPresenterTests
    {
        private Mock<IWindowsHelloRegisterView> _viewMock;
        private Mock<IWindowsHelloManager> _windowsHelloManagerMock;
        private Mock<ILoginManager> _loginManagerMock;
        private WindowsHelloRegisterViewPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _viewMock = new Mock<IWindowsHelloRegisterView>();
            _windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
            _loginManagerMock = new Mock<ILoginManager>();

            _presenter = new WindowsHelloRegisterViewPresenter(_viewMock.Object, _windowsHelloManagerMock.Object, _loginManagerMock.Object);
        }

        [Test]
        public async Task ConfirmEventHandler_ValidPasswords_AuthenticatedWithWindowsHello()
        {
            _viewMock.Setup(v => v.GivenPassword).Returns("password123");
            _viewMock.Setup(v => v.GivenSecondPassword).Returns("password123");
            _windowsHelloManagerMock.Setup(m => m.AuthenticateWithWindowsHelloAsync(It.IsAny<string>())).ReturnsAsync(true);

            _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

            await Task.Delay(100); // Allow the async method to complete

            _loginManagerMock.Verify(m => m.SetPassword("password123"), Times.Once);
            Assert.IsTrue(_presenter.UserSuccessfullyRegistered);
        }

        [Test]
        public async Task ConfirmEventHandler_BlankPasswords_ShowsBlankPasswordError()
        {
            _viewMock.Setup(v => v.GivenPassword).Returns("");
            _viewMock.Setup(v => v.GivenSecondPassword).Returns("");

            _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

            await Task.Delay(100); // Allow the async method to complete

            _viewMock.Verify(v => v.ShowBlankPasswordError(), Times.Once);
            Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
        }

        [Test]
        public async Task ConfirmEventHandler_NonMatchingPasswords_ShowsPasswordMismatchError()
        {
            _viewMock.Setup(v => v.GivenPassword).Returns("password123");
            _viewMock.Setup(v => v.GivenSecondPassword).Returns("password456");

            _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

            await Task.Delay(100); // Allow the async method to complete

            _viewMock.Verify(v => v.ShowPasswordMismatchError(), Times.Once);
            Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
        }

        [Test]
        public async Task ConfirmEventHandler_PasswordTooShort_ShowsPasswordTooShortError()
        {
            _viewMock.Setup(v => v.GivenPassword).Returns("short");
            _viewMock.Setup(v => v.GivenSecondPassword).Returns("short");

            _viewMock.Raise(v => v.ConfirmEvent += null, EventArgs.Empty);

            await Task.Delay(100); // Allow the async method to complete

            _viewMock.Verify(v => v.ShowPasswordTooShortError(), Times.Once);
            Assert.IsFalse(_presenter.UserSuccessfullyRegistered);
        }
    }
}