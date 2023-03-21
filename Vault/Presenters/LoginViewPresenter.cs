using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class LoginViewPresenter
{
    public bool UserSuccessfullyAuthenticated { get; private set; }
    private ILoginView view;
    private ILoginManager passwordLoginManager;
    private IEncryptionManager encryptionManager;

    public LoginViewPresenter(ILoginView view, ILoginManager passwordLoginManager, IEncryptionManager encryptionManager)
    {
        this.view = view;
        this.passwordLoginManager = passwordLoginManager;
        this.view.LoginEvent += LoginEventHandler;
        this.encryptionManager = encryptionManager;
        view.Show();
    }

    public void LoginEventHandler(object? sender, EventArgs e)
    {
        // Get the entered password
        var givenPassword = view.GivenPassword;
        if (string.IsNullOrWhiteSpace(givenPassword))
        {
            view.ShowBlankPasswordGivenError();
            return;
        }

        var validPassword = passwordLoginManager.VerifyPassword(givenPassword);
        if (validPassword)
        {
            encryptionManager.SetEncryptionPassword(givenPassword);
            UserSuccessfullyAuthenticated = true;
            view.Close();
        }
        else
        {
            view.ShowIncorrectPasswordError();
            return;
        }
    }
}