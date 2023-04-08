using Application.Enums;
using Application.Managers;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests
{
    public class SettingsViewPresenterTests
    {
        private Mock<ISettingsView> _settingsViewMock;
        private Mock<IFileManager> _fileManagerMock;
        private Mock<IWindowsHelloManager> _windowsHelloManagerMock;
        private Mock<IPresenterManager> _presenterManagerMock;
        private Mock<ILoginManager> _loginManagerMock;
        private AppSettings _appSettings;
        private SettingsViewPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _settingsViewMock = new Mock<ISettingsView>();
            _fileManagerMock = new Mock<IFileManager>();
            _windowsHelloManagerMock = new Mock<IWindowsHelloManager>();
            _presenterManagerMock = new Mock<IPresenterManager>();
            _loginManagerMock = new Mock<ILoginManager>();
            _appSettings = new AppSettings
            {
                AuthenticationMethod = AuthenticationMethod.Password,
                Mode = ApplicationMode.Basic,
                DeleteUnencryptedFileUponUpload = false,
                DefaultDownloadLocation = "C:\\Downloads"
            };

            _presenter = new SettingsViewPresenter(
                _settingsViewMock.Object,
                _fileManagerMock.Object,
                _appSettings,
                _windowsHelloManagerMock.Object,
                _presenterManagerMock.Object,
                _loginManagerMock.Object);
        }
    }
}