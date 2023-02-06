using Application.Managers;
using Application.Models;
using Application.Views;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Application.Presenters;

public class HomeViewPresenter
{
    private IHomeView view;
    private IFileManager fileManager;
    private BindingSource filesInVaultBindingSource;
    private IEnumerable<EncryptedFile> filesInVault;

    public HomeViewPresenter(IHomeView view, IFileManager fileManager)
    {
        this.filesInVaultBindingSource = new BindingSource();
        this.fileManager = fileManager;
        this.view = view;
        this.view.AddFileToVaultEvent += AddFileToVault;
        this.view.DownloadFileFromVaultEvent += DownloadFileFromVault;

        this.view.SetFilesInVaultListBindingSource(filesInVaultBindingSource);
        LoadAllFilesInVault();
    }

    private void AddFileToVault(object? sender, EventArgs e)
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Select File";
        openFileDialog1.InitialDirectory = @"C:\"; //--"C:\\";
        openFileDialog1.FilterIndex = 2;
        openFileDialog1.Multiselect = false;
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.ShowDialog();

        if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
        {
            bool success = fileManager.AddFileToVault(openFileDialog1.FileName);
            if (success)
            {
                LoadAllFilesInVault();
            }
            else
            {
                MessageBox.Show($"An Error Occurred While Attempting to Encrypt {openFileDialog1.SafeFileName}");
            }
        }
    }

    private void DownloadFileFromVault(object? sender, EventArgs e)
    {
        FileInformation? fileInVault = view.SelectedFile;
        if (fileInVault == null)
        {
            return;
        }

        if (!filesInVault.Any())
        {
            return;
        }

        string fileToDownloadPath = filesInVault.First(_ => _.DecryptedFileInformation != null && _.DecryptedFileInformation == fileInVault).FilePath;

        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.InitialDirectory = "C:\\Users";
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            bool success = fileManager.DownloadFromFromVault(fileToDownloadPath, dialog.FileName);

            if (success)
            {
                string argument = "/select, \"" + dialog.FileName + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
            {
                MessageBox.Show("An error while attempting to download the file!");
                return;
            }
        }
    }

    private void LoadAllFilesInVault()
    {
        filesInVault = fileManager.GetAllFilesInVault();
        filesInVaultBindingSource.DataSource = filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation)
            .OfType<FileInformation>();
    }

    private static string FormatFileSize(string fileLength)
    {
        if (string.IsNullOrEmpty(fileLength))
        {
            return fileLength;
        }

        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = Convert.ToDouble(fileLength);
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        return $"{len:0.##} {sizes[order]}";
    }
}