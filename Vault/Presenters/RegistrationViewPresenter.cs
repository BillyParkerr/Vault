using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class RegistrationViewPresenter
{
    public bool UserSuccessfullyRegistered { get; private set; }

    private readonly ILoginManager _passwordLoginManager;
    private readonly IRegisterView _registerView;

    public RegistrationViewPresenter(ILoginManager passwordLoginManager, IRegisterView registerView)
    {
        _passwordLoginManager = passwordLoginManager;
        _registerView = registerView;
        _registerView.RegisterEvent += RegisterEventHandler;
        _registerView.Show();
    }

    public void RegisterEventHandler(object sender, EventArgs e)
    {
        string enteredPassword = _registerView.GivenPassword;
        string enteredSecondPassword = _registerView.GivenSecondPassword;
        var passwordState = GetPasswordState(enteredPassword, enteredSecondPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                CommitPassword(enteredPassword);
                _registerView.Close();
                return;
            case PasswordState.PasswordNotGiven:
                _registerView.ShowBlankPasswordError();
                return;
            case PasswordState.NonMatching:
                _registerView.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                _registerView.ShowPasswordTooShortError();
                return;
        }
    }

    private void CommitPassword(string password)
    {
        _passwordLoginManager.SetPassword(password);
        UserSuccessfullyRegistered = true;
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