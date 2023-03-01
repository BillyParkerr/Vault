using Application.Managers;
using Application.Views;

namespace Application.Presenters;

public class RegistrationViewPresenter
{
    private IEncryptionManager encryptionManager;
    private IDatabaseManager databaseManager;
    private IRegisterView registerView;

    public RegistrationViewPresenter(IEncryptionManager _encryptionManager, IDatabaseManager _databaseManager,
        IRegisterView _registerView)
    {
        encryptionManager = _encryptionManager;
        databaseManager = _databaseManager;
        registerView = _registerView;
        registerView.RegisterEvent += RegisterEventHandler;
        registerView.Show();
    }

    private enum PasswordState
    {
        NonMatching,
        PasswordNotGiven,
        LengthTooShort,
        Valid
    }

    public void RegisterEventHandler(object? sender, EventArgs e)
    {
        string enteredPassword = registerView.GivenPassword;
        string enteredSecondPassword = registerView.GivenSecondPassword;
        var passwordState = GetPasswordState(enteredPassword, enteredSecondPassword);
        if (passwordState == PasswordState.Valid)
        {
            
        }
        else if (passwordState == PasswordState.PasswordNotGiven)
        {
            registerView.ShowBlankPasswordError();
            return;
        }
        else if (passwordState == PasswordState.NonMatching)
        {
            registerView.ShowPasswordMismatchError();
            return;
        }
        else if (passwordState == PasswordState.LengthTooShort)
        {
            registerView.ShowPasswordTooShortError();
            return;
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