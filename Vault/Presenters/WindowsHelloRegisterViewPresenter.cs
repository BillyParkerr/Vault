using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class WindowsHelloRegisterViewPresenter
{
    public bool UserSuccessfullyRegistered { get; private set; }

    private readonly IWindowsHelloRegisterView _view;
    private readonly IWindowsHelloManager _windowsHelloManager;
    private readonly ILoginManager _loginManager;

    public WindowsHelloRegisterViewPresenter(IWindowsHelloRegisterView view, IWindowsHelloManager windowsHelloManager, ILoginManager loginManager)
    {
        _windowsHelloManager = windowsHelloManager;
        _loginManager = loginManager;
        _view = view;
        _view.ConfirmEvent += ConfirmEventHandler;
    }

    private async void ConfirmEventHandler(object _, EventArgs __)
    {
        string enteredPassword = _view.GivenPassword;
        string enteredSecondPassword = _view.GivenSecondPassword;
        var passwordState = GetPasswordState(enteredPassword, enteredSecondPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                await CommitPasswordAsync(enteredPassword);
                _view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                _view.ShowBlankPasswordError();
                return;
            case PasswordState.NonMatching:
                _view.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                _view.ShowPasswordTooShortError();
                return;
        }
    }

    private async Task CommitPasswordAsync(string password)
    {
        bool authenticated = await _windowsHelloManager.AuthenticateWithWindowsHelloAsync("Please authenticate to register your backup password.");
        if (authenticated)
        {
            _loginManager.SetPassword(password);
            UserSuccessfullyRegistered = true;
        }
        else
        {
            UserSuccessfullyRegistered = false;
        }
    }

    /// <summary>
    /// Ensure the password is given and meets security requirements.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="secondPassword"></param>
    /// <returns></returns>
    private static PasswordState GetPasswordState(string password, string secondPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(secondPassword))
        {
            return PasswordState.PasswordNotGiven;
        }

        if (password != secondPassword)
        {
            return PasswordState.NonMatching;
        }

        if (password.Length <= 6)
        {
            return PasswordState.LengthTooShort;
        }

        return PasswordState.Valid;
    }
}