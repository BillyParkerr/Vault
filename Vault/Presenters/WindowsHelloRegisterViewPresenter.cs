using Application.Enums;
using Application.Managers;
using Application.Views;
using Microsoft.Win32;

namespace Application.Presenters;

public class WindowsHelloRegisterViewPresenter
{
    public bool UserSuccessfullyRegistered { get; private set; }

    private IWindowsHelloRegisterView view;
    private IWindowsHelloManager windowsHelloManager;
    private ILoginManager loginManager;

    public WindowsHelloRegisterViewPresenter(IWindowsHelloRegisterView view, IWindowsHelloManager windowsHelloManager, ILoginManager loginManager)
    {
        this.windowsHelloManager = windowsHelloManager;
        this.loginManager = loginManager;
        this.view = view;
        this.view.ConfirmEvent += ConfirmEventHandler;
    }

    private async void ConfirmEventHandler(object _, EventArgs __)
    {
        string enteredPassword = view.GivenPassword;
        string enteredSecondPassword = view.GivenSecondPassword;
        var passwordState = GetPasswordState(enteredPassword, enteredSecondPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                await CommitPasswordAsync(enteredPassword);
                view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                view.ShowBlankPasswordError();
                return;
            case PasswordState.NonMatching:
                view.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                view.ShowPasswordTooShortError();
                return;
        }
    }

    private async Task CommitPasswordAsync(string password)
    {
        bool authenticated = await windowsHelloManager.AuthenticateWithWindowsHelloAsync("Please authenticate to register your backup password.");
        if (authenticated)
        {
            loginManager.SetPassword(password);
            UserSuccessfullyRegistered = true;
        }
        else
        {
            UserSuccessfullyRegistered = false;
        }
    }

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