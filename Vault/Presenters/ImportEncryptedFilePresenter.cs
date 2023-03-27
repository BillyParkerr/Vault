using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ImportEncryptedFilePresenter
{
    private readonly IImportEncryptedFileView View;
    private readonly IFileManager fileManager;
    private readonly string EncryptedFilePath;

    public ImportEncryptedFilePresenter(IImportEncryptedFileView view, IFileManager fileManager)
    {
        View = view;
        this.fileManager = fileManager;
        EncryptedFilePath = this.fileManager.GetFilePathFromExplorer("AES files (*.aes)|*.aes");
        if (string.IsNullOrWhiteSpace(EncryptedFilePath))
        {
            return;
        }
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
                ImportFileToVault(givenPassword);
                View.Close();
                return;
            case PasswordState.PasswordNotGiven:
                View.ShowBlankPasswordError();
                return;
        }
    }

    private void ImportFileToVault(string password)
    {
        var success = fileManager.ImportEncryptedFileToVault(EncryptedFilePath, password);
        if (!success)
        {
            MessageBox.Show($"An Error Occurred While Attempting to import the selected file. Perhaps the password was incorrect?");
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