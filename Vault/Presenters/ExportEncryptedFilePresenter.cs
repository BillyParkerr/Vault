﻿using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class ExportEncryptedFilePresenter
{
    private readonly IExportEncryptedFileView _view;
    private readonly IFileManager _fileManager;
    private readonly EncryptedFile _encryptedFileToExport;

    public ExportEncryptedFilePresenter(IExportEncryptedFileView view, IFileManager fileManager, EncryptedFile encryptedFile)
    {
        _encryptedFileToExport = encryptedFile;
        this._view = view;
        this._fileManager = fileManager;
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
                ExportEncryptedFile(givenPassword);
                _view.Close();
                return;
            case PasswordState.PasswordNotGiven:
                _view.ShowBlankPasswordError();
                return;
            case PasswordState.LengthTooShort:
                _view.ShowPasswordTooShortError();
                return;
        }
    }

    private void ExportEncryptedFile(string password)
    {
        string selectedPath = _fileManager.GetFolderPathFromExplorer();
        bool success = _fileManager.DownloadEncryptedFileFromVault(_encryptedFileToExport.FilePath, selectedPath, password);
        if (success)
        {
            _fileManager.OpenFolderInExplorer(selectedPath);
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