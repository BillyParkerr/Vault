using Application.Enums;
using Application.EventArguments;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ChangePasswordViewPresenter
{
    public event EventHandler<PasswordChangedEventArgs> NewPasswordChosen;

    private readonly ILoginManager loginManager;
    private readonly IChangePasswordView view;

    public ChangePasswordViewPresenter(ILoginManager loginManager, IChangePasswordView view)
    {
        this.loginManager = loginManager;
        this.view = view;
        this.view.ConfirmPasswordEvent += ConfirmPasswordEventHandler;
        this.view.Show();
    }

    private void ConfirmPasswordEventHandler(object sender, EventArgs e)
    {
        string enteredOldPassword = view.GivenOldPassword;
        var oldPasswordState = GetOldPasswordState(enteredOldPassword);
        switch (oldPasswordState)
        {
            case PasswordState.PasswordNotGiven:
                view.ShowBlankOldPasswordError();
                return;
            case PasswordState.Incorrect:
                view.ShowIncorrectOldPasswordError();
                return;
        }

        string enteredNewPassword = view.GivenNewPassword;
        string enteredSecondNewPassword = view.GivenSecondNewPassword;
        var passwordState = GetNewPasswordState(enteredNewPassword, enteredSecondNewPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                NewPasswordChosen?.Invoke(this, new(enteredOldPassword, enteredNewPassword));
                view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                view.ShowBlankNewPasswordError();
                return;
            case PasswordState.NonMatching:
                view.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                view.ShowNewPasswordTooShortError();
                return;
        }
    }

    private PasswordState GetOldPasswordState(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordState.PasswordNotGiven;
        }

        if (loginManager.VerifyPassword(password) == false)
        {
            return PasswordState.Incorrect;
        }

        return PasswordState.Valid;
    }

    private static PasswordState GetNewPasswordState(string password, string secondPassword)
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