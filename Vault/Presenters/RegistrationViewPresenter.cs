using Application.Enums;
using Application.Managers;
using Application.Views;

namespace Application.Presenters;

public class RegistrationViewPresenter
{
    public bool UserSuccessfullyRegistered { get; private set; }
    public AuthenticationMethod ChosenAuthenticationMethod { get; private set; }

    private IEncryptionManager encryptionManager;
    private IRegisterView registerView;

    public RegistrationViewPresenter(IEncryptionManager _encryptionManager, IRegisterView _registerView)
    {
        encryptionManager = _encryptionManager;
        registerView = _registerView;
        registerView.RegisterEvent += RegisterEventHandler;
        registerView.Show();
    }

    public void RegisterEventHandler(object? sender, EventArgs e)
    {
        string enteredPassword = registerView.GivenPassword;
        string enteredSecondPassword = registerView.GivenSecondPassword;
        var passwordState = GetPasswordState(enteredPassword, enteredSecondPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                CommitPassword(enteredPassword);
                registerView.Close();
                return;
            case PasswordState.PasswordNotGiven:
                registerView.ShowBlankPasswordError();
                return;
            case PasswordState.NonMatching:
                registerView.ShowPasswordMismatchError();
                return;
            case PasswordState.LengthTooShort:
                registerView.ShowPasswordTooShortError();
                return;
        }
    }

    private void CommitPassword(string password)
    {
        encryptionManager.SetPassword(password); // EncryptionManager will also take care of commiting this to the database.
        UserSuccessfullyRegistered = true;
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