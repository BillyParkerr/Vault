using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ExportEncryptedFilePresenter
{
    private readonly IExportEncryptedFileView view;
    private readonly IFileManager fileManager;
    private readonly EncryptedFile encryptedFileToExport;

    public ExportEncryptedFilePresenter(IExportEncryptedFileView view, IFileManager fileManager, EncryptedFile encryptedFile)
    {
        encryptedFileToExport = encryptedFile;
        this.view = view;
        this.fileManager = fileManager;
        view.ConfirmEvent += ConfirmEventHandler;
        view.Show();
    }

    private void ConfirmEventHandler(object sender, EventArgs e)
    {
        var givenPassword = view.GivenPassword;
        var passwordState = GetPasswordState(givenPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                ExportEncryptedFile(givenPassword);
                view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                view.ShowBlankPasswordError();
                return;
            case PasswordState.LengthTooShort:
                view.ShowPasswordTooShortError();
                return;
        }
    }

    private void ExportEncryptedFile(string password)
    {
        string selectedPath = fileManager.GetFolderPathFromExplorer();
        bool success = fileManager.DownloadEncryptedFileFromVault(encryptedFileToExport.FilePath, selectedPath, password);
        if (success)
        {
            fileManager.OpenFolderInExplorer(selectedPath);
        }
        else
        {
            MessageBox.Show("There was a problem exporting the selected file!");
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