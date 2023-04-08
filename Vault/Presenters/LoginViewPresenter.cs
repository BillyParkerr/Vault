using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class LoginViewPresenter
{
    public bool UserSuccessfullyAuthenticated { get; private set; }
    private readonly ILoginView _view;
    private readonly ILoginManager _passwordLoginManager;
    private readonly IEncryptionManager _encryptionManager;

    public LoginViewPresenter(ILoginView view, ILoginManager passwordLoginManager, IEncryptionManager encryptionManager)
    {
        this._view = view;
        this._passwordLoginManager = passwordLoginManager;
        this._view.LoginEvent += LoginEventHandler;
        this._encryptionManager = encryptionManager;
        view.Show();
    }

    public void LoginEventHandler(object sender, EventArgs e)
    {
        // Get the entered password
        var givenPassword = _view.GivenPassword;
        if (string.IsNullOrWhiteSpace(givenPassword))
        {
            _view.ShowBlankPasswordGivenError();
            return;
        }

        var validPassword = _passwordLoginManager.VerifyPassword(givenPassword);
        if (validPassword)
        {
            _encryptionManager.SetEncryptionPassword(givenPassword);
            UserSuccessfullyAuthenticated = true;
            _view.Close();
        }
        else
        {
            _view.ShowIncorrectPasswordError();
            return;
        }
    }
}