using Application.Enums;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ImportEncryptedFilePresenter
{
    public event EventHandler<string> PasswordEntered; 
    private IImportEncryptedFileView View;

    public ImportEncryptedFilePresenter(IImportEncryptedFileView view)
    {
        View = view;
        view.ConfirmEvent += ConfirmEventHandler;
        view.Show();
    }

    private void ConfirmEventHandler(object sender, EventArgs e)
    {
        var givenPassword = View.GivenPassword;
        var passwordState = GetPasswordState(givenPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                PasswordEntered?.Invoke(this, givenPassword);
                View.Close();
                return;
            case PasswordState.PasswordNotGiven:
                View.ShowBlankPasswordError();
                return;
        }
    }

    private static PasswordState GetPasswordState(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordState.PasswordNotGiven;
        }

        return PasswordState.Valid;
    }
}