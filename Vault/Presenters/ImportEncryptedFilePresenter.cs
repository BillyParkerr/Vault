using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ImportEncryptedFilePresenter
{
    private readonly IImportEncryptedFileView _view;
    private readonly IFileManager _fileManager;
    private readonly string _encryptedFilePath;

    public ImportEncryptedFilePresenter(IImportEncryptedFileView view, IFileManager fileManager)
    {
        _view = view;
        _fileManager = fileManager;
        _encryptedFilePath = _fileManager.GetFilePathFromExplorer("AES files (*.aes)|*.aes");
        if (string.IsNullOrWhiteSpace(_encryptedFilePath))
        {
            return;
        }
        view.ConfirmEvent += ConfirmEventHandler;
        view.Show();
    }

    private void ConfirmEventHandler(object sender, EventArgs e)
    {
        var givenPassword = _view.GivenPassword;
        var passwordState = GetPasswordState(givenPassword);
        switch (passwordState)
        {
            case PasswordState.Valid:
                ImportFileToVault(givenPassword);
                _view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                _view.ShowBlankPasswordError();
                return;
        }
    }

    private void ImportFileToVault(string password)
    {
        var success = _fileManager.ImportEncryptedFileToVault(_encryptedFilePath, password);
        if (!success)
        {
            MessageBox.Show($"An Error Occurred While Attempting to import the selected file. Perhaps the password was incorrect?");
        }
    }

    /// <summary>
    /// Ensure the password is given
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private static PasswordState GetPasswordState(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordState.PasswordNotGiven;
        }

        return PasswordState.Valid;
    }
}