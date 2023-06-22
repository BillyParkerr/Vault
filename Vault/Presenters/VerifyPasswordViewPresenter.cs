using Application.EventArguments;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class VerifyPasswordViewPresenter
{
    public EventHandler<PasswordVerificationCompleteEventArgs> PasswordVerificationFinished;
    private readonly IVerifyPasswordView _view;
    private readonly ILoginManager _passwordLoginManager;

    public VerifyPasswordViewPresenter(IVerifyPasswordView view, ILoginManager passwordLoginManager)
    {
        _view = view;
        _passwordLoginManager = passwordLoginManager;
        _view.VerifyEvent += VerifyEventHandler;
        _view.UserClosedFormEvent += UserClosedFormEventHandler;
        view.Show();
    }

    private void VerifyEventHandler(object sender, EventArgs e)
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
            PasswordVerificationFinished.Invoke(this, new PasswordVerificationCompleteEventArgs(true, givenPassword));
            _view.Close();
        }
        else
        {
            _view.ShowIncorrectPasswordError();
        }
    }

    private void UserClosedFormEventHandler(object sender, EventArgs e)
    {
        PasswordVerificationFinished.Invoke(this, new PasswordVerificationCompleteEventArgs(false, null));
    }
}