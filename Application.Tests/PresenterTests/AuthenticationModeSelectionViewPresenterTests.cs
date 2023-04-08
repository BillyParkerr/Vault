using Application.Enums;
using Application.Presenters;
using Application.Views.Interfaces;
using Moq;

namespace Application.Tests.PresenterTests;

[TestFixture]
public class AuthenticationModeSelectionViewPresenterTests
{
    private Mock<IAuthenticationModeSelectionView> _viewMock;
    private AppSettings _appSettings;
    private AuthenticationModeSelectionViewPresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _viewMock = new Mock<IAuthenticationModeSelectionView>();
        _appSettings = new AppSettings();
        _presenter = new AuthenticationModeSelectionViewPresenter(_viewMock.Object, _appSettings);
    }

    [Test]
    public void PasswordModeSelectedEventHandler_UpdatesAppSettingsAndClosesView()
    {
        // Arrange
        _viewMock.Raise(v => v.PasswordModeSelected += null, EventArgs.Empty);

        // Assert
        Assert.AreEqual(AuthenticationMethod.Password, _appSettings.AuthenticationMethod);
        _viewMock.Verify(v => v.Close(), Times.Once);
    }

    [Test]
    public void WindowsHelloModeSelectedEventHandler_UpdatesAppSettingsAndClosesView()
    {
        // Arrange
        _viewMock.Raise(v => v.WindowsHelloModeSelected += null, EventArgs.Empty);

        // Assert
        Assert.AreEqual(AuthenticationMethod.WindowsHello, _appSettings.AuthenticationMethod);
        _viewMock.Verify(v => v.Close(), Times.Once);
    }
}