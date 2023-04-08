using Application.Enums;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class AuthenticationModeSelectionViewPresenter
{
    private readonly IAuthenticationModeSelectionView _view;
    private readonly AppSettings _appSettings;

    public AuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView view, AppSettings appSettings)
    {
        this._view = view;
        this._view.PasswordModeSelected += PasswordModeSelectedEventHandler;
        this._view.WindowsHelloModeSelected += WindowsHelloModeSelectedEventHandler;
        this._appSettings = appSettings;
    }

    private void PasswordModeSelectedEventHandler(object _, EventArgs __)
    {
        _appSettings.AuthenticationMethod = AuthenticationMethod.Password;
        Program.UpdateAppSettings(_appSettings);
        _view.Close();
    }

    private void WindowsHelloModeSelectedEventHandler(object _, EventArgs __)
    {
        _appSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
        Program.UpdateAppSettings(_appSettings);
        _view.Close();
    }
}