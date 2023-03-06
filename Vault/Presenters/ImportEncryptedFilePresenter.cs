using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Presenters;

public class ImportEncryptedFilePresenter
{
    public event EventHandler<string> PasswordEntered; 
    private IImportEncryptedFileView View;
    private IFileManager FileManager;

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