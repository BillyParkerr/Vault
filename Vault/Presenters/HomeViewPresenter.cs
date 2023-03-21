using Application.Managers;
using Application.Models;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class HomeViewPresenter
{
    private IHomeView view;
    private IFileManager fileManager;
    private IDatabaseManager databaseManager;
    private BindingSource filesInVaultBindingSource;
    private IEnumerable<EncryptedFile> filesInVault;

    public HomeViewPresenter(IHomeView view, IFileManager fileManager, IDatabaseManager databaseManager)
    {
        this.filesInVaultBindingSource = new BindingSource();
        this.fileManager = fileManager;
        this.view = view;

        this.databaseManager = databaseManager;
        databaseManager.vaultContentsChangedEvent += LoadAllFilesInVault;
        // Subscribe to events
        this.view.AddFileToVaultEvent += AddFileToVaultEventHandler;
        this.view.AddFolderToVaultEvent += AddFolderToVaultEventHandler;
        this.view.DownloadFileFromVaultEvent += DownloadFileFromVaultEventHandler;
        this.view.DeleteFileFromVaultEvent += DeleteFileFromVaultEventHander;
        this.view.OpenFileFromVaultEvent += OpenFileFromVaultEventHandler;
        this.view.ImportFileToVaultEvent += ImportFileToVaultEventHandler;
        this.view.ExportFileFromVaultEvent += ExportFileFromVaultEventHandler;
        this.view.FormClosingEvent += FormClosingEventHandler;
        this.view.SearchFilterAppliedEvent += SearchFilterAppliedEventHandler;
        this.view.SetFilesInVaultListBindingSource(filesInVaultBindingSource);
        LoadAllFilesInVault();
    }

    private static T RunOnSTAThread<T>(Func<T> func)
    {
        T result = default(T);
        var thread = new Thread(() => { result = func(); });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        return result;
    }

    private void AddFileToVaultEventHandler(object sender, EventArgs e)
    {
        string fileToAdd = RunOnSTAThread(() => GetFileFromExplorer("All Files (*.*)|*.*"));
        if (fileToAdd == null)
        {
            return;
        }

        bool success = fileManager.AddFileToVault(fileToAdd);
        if (!success)
        {
            MessageBox.Show($"An Error Occurred While Attempting to Encrypt {fileToAdd}");
        }
    }

    private void AddFolderToVaultEventHandler(object sender, EventArgs e)
    {
        var folderToAdd = RunOnSTAThread(() => GetFolderFromExplorer());
        if (folderToAdd != null)
        {
            bool success = fileManager.ZipFolderAndAddToVault(folderToAdd);
            if (!success)
            {
                MessageBox.Show($"An Error Occurred While Attempting to Zip & Encrypt the folder.");
            }
        }
    }

    private void DownloadFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var filePath = GetSelectedFilePath();
        if (filePath == null)
        {
            return;
        }

        string selectedPath = RunOnSTAThread(() => GetFolderFromExplorer());
        bool success = fileManager.DownloadFileFromVault(filePath, selectedPath);

        if (success)
        {
            string argument = "/select, \"" + selectedPath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
        else
        {
            MessageBox.Show("An error occurred while attempting to download the file!");
            return;
        }
    }

    private void OpenFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var filePath = GetSelectedFilePath();
        if (filePath == null)
        {
            return;
        }

        fileManager.OpenFileFromVaultAndReencryptUponClosure(filePath);
    }

    private void DeleteFileFromVaultEventHander(object? sender, EventArgs e)
    {
        var filePath = GetSelectedFilePath();
        if (filePath == null)
        {
            return;
        }
        bool success = fileManager.DeleteFileFromVault(filePath);
        if (!success)
        {
            view.ShowFailedToDeleteError();
        }
    }

    private void ExportFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var selectedEncryptedFile = GetSelectedEncryptedFile();
        var exportEncryptedFileView = Program.container.GetInstance<IExportEncryptedFileView>();

        var exportEncryptedFilePresenter = new ExportEncryptedFilePresenter(exportEncryptedFileView);
        exportEncryptedFilePresenter.PasswordEntered += (_, password) =>
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                SaveEncryptedFile(password, selectedEncryptedFile);
            }
        };
    }

    private void SearchFilterAppliedEventHandler(object sender, EventArgs e)
    {
        string searchValue = view.SearchValue;
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            ApplySearchFilter(searchValue);
        }
        else
        {
            LoadAllFilesInVault();
        }
    }

    private void ApplySearchFilter(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return;
        }

        filter = filter.ToLower().Replace("'", "''").Replace("[", "[[]");
        var filesInVault = filesInVaultBindingSource.List.OfType<FileInformation>();
        filesInVault = filesInVault.Where(x => x.FileName.ToLower().Contains(filter) || x.FileExtension.ToLower().Contains(filter));
        filesInVaultBindingSource.DataSource = filesInVault;
    }

    private void SaveEncryptedFile(string password, EncryptedFile encryptedFileToExport)
    {
        string selectedPath = RunOnSTAThread(() => GetFolderFromExplorer());
        bool success = fileManager.DownloadEncryptedFileFromVault(encryptedFileToExport.FilePath, selectedPath, password);
        if (success)
        {
            System.Diagnostics.Process.Start("explorer.exe", selectedPath);
        }
        else
        {
            MessageBox.Show("There was a problem exporting the selected file!");
        }
    }

    private void ImportFileToVaultEventHandler(object sender, EventArgs e)
    {
        var fileToImportPath = RunOnSTAThread(() => GetFileFromExplorer("AES files (*.aes)|*.aes"));
        if (fileToImportPath == null)
        {
            return;
        }

        if (Path.GetExtension(fileToImportPath) != ".aes")
        {
            MessageBox.Show("The chosen file is not an encrypted file. Please add it to the vault via the Add File To Vault button.");
            return;
        }

        var importEncryptedFileView = Program.container.GetInstance<IImportEncryptedFileView>();
        var importEncryptedFilePresenter = new ImportEncryptedFilePresenter(importEncryptedFileView);
        importEncryptedFilePresenter.PasswordEntered += (_, password) =>
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                var success = fileManager.ImportEncryptedFileToVault(fileToImportPath, password);
                if (!success)
                {
                    MessageBox.Show($"An Error Occurred While Attempting to import the selected file. Perhaps the password was incorrect?");
                }
            }
        };
    }

    private static string GetFileFromExplorer(string filter = null)
    {
        string result = null;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Select File";
        openFileDialog1.InitialDirectory = @"C:\"; //--"C:\\";
        openFileDialog1.Filter = filter;
        openFileDialog1.FilterIndex = 1;
        openFileDialog1.Multiselect = false;
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.ShowDialog();

        if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
        {
            result = openFileDialog1.FileName;
        }

        return result;
    }

    private static string GetFolderFromExplorer()
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Select Folder";
        folderBrowserDialog.ShowNewFolderButton = false;
        folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
        DialogResult result = folderBrowserDialog.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
        {
            return folderBrowserDialog.SelectedPath;
        }

        return null;
    }

    private void LoadAllFilesInVault(object? o, EventArgs? e)
    {
        filesInVault = fileManager.GetAllFilesInVault();
        var fileInfo = filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation)
            .OfType<FileInformation>();
        var newBindSource = new BindingSource();
        newBindSource.DataSource = fileInfo;
        this.filesInVaultBindingSource = newBindSource;
        view.SetFilesInVaultListBindingSource(filesInVaultBindingSource);

        // TODO reapply the search filter here
    }

    private void LoadAllFilesInVault()
    {
        filesInVault = fileManager.GetAllFilesInVault();
        var fileInfo = filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation)
            .OfType<FileInformation>();
        filesInVaultBindingSource.DataSource = fileInfo;
        ApplySearchFilter(view.SearchValue);
    }

    private void FormClosingEventHandler(object sender, FormClosingEventArgs e)
    {
        fileManager.CleanupTempFiles();
    }

    private string GetSelectedFilePath()
    {
        FileInformation fileInVault = view.SelectedFile;
        if (fileInVault == null)
        {
            return null;
        }

        if (!filesInVault.Any())
        {
            return null;
        }

        return filesInVault.First(_ => _.DecryptedFileInformation != null && _.DecryptedFileInformation == fileInVault).FilePath;
    }

    private EncryptedFile GetSelectedEncryptedFile()
    {
        FileInformation? fileInVault = view.SelectedFile;
        if (fileInVault == null)
        {
            return null;
        }

        if (!filesInVault.Any())
        {
            return null;
        }

        return filesInVault.First(_ => _.DecryptedFileInformation != null && _.DecryptedFileInformation == fileInVault);
    }
}