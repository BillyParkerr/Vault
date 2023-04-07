using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class HomeViewPresenter
{
    private readonly IHomeView view;
    private readonly IFileManager fileManager;
    private readonly IPresenterManager presenterManager;
    private BindingSource filesInVaultBindingSource;
    private IEnumerable<EncryptedFile> filesInVault;
    private readonly AppSettings appSettings;

    public HomeViewPresenter(IHomeView view, IFileManager fileManager, IDatabaseManager databaseManager, IPresenterManager presenterManager, AppSettings appSettings)
    {
        this.filesInVaultBindingSource = new BindingSource();
        this.fileManager = fileManager;
        this.view = view;
        this.presenterManager = presenterManager;
        this.appSettings = appSettings;

        databaseManager.vaultContentsChangedEvent += LoadAllFilesInVault;
        // Subscribe to events
        this.view.AddFileToVaultEvent += AddFileToVaultEventHandler;
        this.view.AddFolderToVaultEvent += AddFolderToVaultEventHandler;
        this.view.DownloadFileFromVaultEvent += DownloadFileFromVaultEventHandler;
        this.view.DeleteFileFromVaultEvent += DeleteFileFromVaultEventHandler;
        this.view.OpenFileFromVaultEvent += OpenFileFromVaultEventHandler;
        this.view.ImportFileToVaultEvent += ImportFileToVaultEventHandler;
        this.view.ExportFileFromVaultEvent += ExportFileFromVaultEventHandler;
        this.view.FormClosingEvent += FormClosingEventHandler;
        this.view.SearchFilterAppliedEvent += SearchFilterAppliedEventHandler;
        this.view.OpenSettingsEvent += OpenSettingsEventHandler;
        this.view.SetFilesInVaultListBindingSource(filesInVaultBindingSource);
        LoadAllFilesInVault();
        ConfigureViewBasedUponAppSettings();
    }

    private void ConfigureViewBasedUponAppSettings()
    {
        if (appSettings.Mode == ApplicationMode.Advanced)
        {
            view.SetAdvancedModeView();
        }
        else
        {
            view.SetBasicModeView();
        }
    }

    private void OpenSettingsEventHandler(object _, EventArgs e)
    {
        view.PauseView();
        var settingsViewPresenter = presenterManager.GetSettingsViewPresenter();
        settingsViewPresenter.SettingsConfirmed += (_, _) =>
        {
            view.ResumeView();
            ConfigureViewBasedUponAppSettings();
        };
    }

    private void AddFileToVaultEventHandler(object sender, EventArgs e)
    {
        string fileToAdd = fileManager.GetFilePathFromExplorer("All Files (*.*)|*.*");
        if (fileToAdd == null)
        {
            return;
        }

        bool success = fileManager.AddFileToVault(fileToAdd);
        if (!success)
        {
            ShowMessageBox($"An Error Occurred While Attempting to Encrypt {fileToAdd}");
        }
    }

    private void AddFolderToVaultEventHandler(object sender, EventArgs e)
    {
        var folderToAdd = fileManager.GetFolderPathFromExplorer();
        if (folderToAdd != null)
        {
            bool success = fileManager.ZipFolderAndAddToVault(folderToAdd);
            if (!success)
            {
                ShowMessageBox("An Error Occurred While Attempting to Zip & Encrypt the folder.");
            }
        }
    }

    private void DownloadFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var filePath = GetSelectedFilePath();
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        string selectedPath = fileManager.GetFolderPathFromExplorer();
        if (string.IsNullOrWhiteSpace(selectedPath))
        {
            return;
        }

        bool success = fileManager.DownloadFileFromVault(filePath, selectedPath);

        if (success)
        {
            fileManager.OpenFolderInExplorer(selectedPath);
        }
        else
        {
            ShowMessageBox("An error occurred while attempting to download the file!");
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

    private void DeleteFileFromVaultEventHandler(object sender, EventArgs e)
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
        if (selectedEncryptedFile == null)
        {
            return;
        }

        presenterManager.GetExportEncryptedFilePresenter(selectedEncryptedFile);
        // TODO Potentially pause view while Export view is open
    }

    private void ImportFileToVaultEventHandler(object sender, EventArgs e)
    {
        presenterManager.GetImportEncryptedFilePresenter();
        // TODO Potentially pause view while Import view is open
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

    private void LoadAllFilesInVault(object o, EventArgs e)
    {
        filesInVault = fileManager.GetAllFilesInVault();
        var fileInfo = filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation);
        var newBindSource = new BindingSource();
        newBindSource.DataSource = fileInfo;
        filesInVaultBindingSource = newBindSource;
        view.SetFilesInVaultListBindingSource(filesInVaultBindingSource);
    }

    private void LoadAllFilesInVault()
    {
        filesInVault = fileManager.GetAllFilesInVault();
        var fileInfo = filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation);
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
        FileInformation fileInVault = view.SelectedFile;
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

    protected virtual void ShowMessageBox(string message)
    {
        MessageBox.Show(message);
    }
}