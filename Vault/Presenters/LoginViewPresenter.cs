using Application.Managers;
using Application.Views;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.ComponentModel;
using System.Xml.Linq;

namespace Application.Presenters;

public class LoginViewPresenter
{
    public bool UserSuccessfullyAuthenticated { get; private set; }
    private ILoginView view;
    private IEncryptionManager encryptionManager;

    public LoginViewPresenter(ILoginView view, IEncryptionManager encryptionManager)
    {
        this.view = view;
        this.encryptionManager = encryptionManager;
        this.view.LoginEvent += LoginEventHandler;
        view.Show();
    }

    public void LoginEventHandler(object? sender, EventArgs e)
    {
        // Get the entered password
        var givenPassword = view.GivenPassword;
        if (string.IsNullOrWhiteSpace(givenPassword))
        {
            // TODO implement an error screen
            return;
        }

        var validPassword = encryptionManager.VerifyPassword(givenPassword);
        if (validPassword)
        {
            UserSuccessfullyAuthenticated = true;
            view.Close();
        }
        else
        {
            // TODO Show error and remove this code
        }
    }
}