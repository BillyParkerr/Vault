using Application.Enums;
using Application.Views;

namespace Application.Presenters;

public class AuthenticationModeSelectionViewPresenter
{
    private IAuthenticationModeSelectionView view;
    private AppSettings appSettings;

    public AuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView view, AppSettings appSettings)
    {
        this.view = view;
        this.view.PasswordModeSelected += PasswordModeSelectedEventHandler;
        this.view.WindowsHelloModeSelected += WindowsHelloModeSelectedEventHandler;
        this.appSettings = appSettings;
    }

    private void PasswordModeSelectedEventHandler(object _, EventArgs __)
    {
        appSettings.AuthenticationMethod = AuthenticationMethod.Password;
        Program.UpdateAppSettings(appSettings);
        view.Close();
    }

    private void WindowsHelloModeSelectedEventHandler(object _, EventArgs __)
    {
        appSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
        Program.UpdateAppSettings(appSettings);
        view.Close();
    }
}