using Application.Enums;
using Application.EventArguments;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ChangePasswordViewPresenter
{
    public event EventHandler<PasswordChangedEventArgs> NewPasswordChosen;

    private readonly ILoginManager _loginManager;
    private readonly IChangePasswordView _view;

    public ChangePasswordViewPresenter(ILoginManager loginManager, IChangePasswordView view)
    {
        this._loginManager = loginManager;
        this._view = view;
        this._view.ConfirmPasswordEvent += ConfirmPasswordEventHandler;
        this._view.Show();
    }

    private void ConfirmPasswordEventHandler(object sender, EventArgs e)
    {
        string enteredOldPassword = _view.GivenOldPassword;
        var oldPasswordState = GetOldPasswordState(enteredOldPassword);
        switch (oldPasswordState)
        {
            case PasswordState.PasswordNotGiven:
                _view.ShowBlankOldPasswordError();
                return;
            case PasswordState.Incorrect:
                _view.ShowIncorrectOldPasswordError();
                return;
        }

        string enteredNewPassword = _view.GivenNewPassword;
        string enteredSecondNewPassword = _view.GivenSecondNewPassword;
        var passwordState = GetNewPasswordState(enteredNewPassword, enteredSecondNewPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                NewPasswordChosen?.Invoke(this, new(enteredOldPassword, enteredNewPassword));
                _view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                _view.ShowBlankNewPasswordError();
                return;
            case PasswordState.NonMatching:
                _view.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                _view.ShowNewPasswordTooShortError();
                return;
        }
    }

    private PasswordState GetOldPasswordState(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordState.PasswordNotGiven;
        }

        if (_loginManager.VerifyPassword(password) == false)
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