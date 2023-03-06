using Application.Enums;
using Application.Views;

namespace Application.Presenters;

public class ExportEncryptedFilePresenter
{
    private IExportEncryptedFileView View;
    public event EventHandler<string> PasswordEntered;

    public ExportEncryptedFilePresenter(IExportEncryptedFileView view)
    {
        View = view;
        view.ConfirmEvent += ConfirmEventHandler;
        view.Show();
    }

    private void ConfirmEventHandler(object? sender, EventArgs e)
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
            case PasswordState.LengthTooShort:
                View.ShowPasswordTooShortError();
                return;
        }
    }

    private static PasswordState GetPasswordState(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordState.PasswordNotGiven;
        }

        if (password.Length <= 6)
        {
            return PasswordState.LengthTooShort;
        }

        return PasswordState.Valid;
    }
}