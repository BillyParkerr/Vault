using Application.Enums;
using Application.Views.Interfaces;

namespace Application.Presenters;

/// <summary>
/// This presenter class is responvile for handing events invoked by the AuthenticationModeSelectionView
///
/// When the user selects which authentication method to use from the user. This is then committed to the AppSettings.json file.
/// </summary>
public class AuthenticationModeSelectionViewPresenter
{
    private readonly IAuthenticationModeSelectionView _view;
    private readonly AppSettings _appSettings;

    public AuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView view, AppSettings appSettings)
    {
        _view = view;
        _view.PasswordModeSelected += PasswordModeSelectedEventHandler;
        _view.WindowsHelloModeSelected += WindowsHelloModeSelectedEventHandler;
        _appSettings = appSettings;
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