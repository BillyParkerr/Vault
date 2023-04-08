using Application.Enums;
using Application.Managers;
using Application.Models;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class HomeViewPresenter
{
    private readonly IHomeView _view;
    private readonly IFileManager _fileManager;
    private readonly IPresenterManager _presenterManager;
    private BindingSource _filesInVaultBindingSource;
    private IEnumerable<EncryptedFile> _filesInVault;
    private readonly AppSettings _appSettings;

    public HomeViewPresenter(IHomeView view, IFileManager fileManager, IDatabaseManager databaseManager, IPresenterManager presenterManager, AppSettings appSettings)
    {
        this._filesInVaultBindingSource = new BindingSource();
        this._fileManager = fileManager;
        this._view = view;
        this._presenterManager = presenterManager;
        this._appSettings = appSettings;

        databaseManager.VaultContentsChangedEvent += LoadAllFilesInVault;
        // Subscribe to events
        this._view.AddFileToVaultEvent += AddFileToVaultEventHandler;
        this._view.AddFolderToVaultEvent += AddFolderToVaultEventHandler;
        this._view.DownloadFileFromVaultEvent += DownloadFileFromVaultEventHandler;
        this._view.DeleteFileFromVaultEvent += DeleteFileFromVaultEventHandler;
        this._view.OpenFileFromVaultEvent += OpenFileFromVaultEventHandler;
        this._view.ImportFileToVaultEvent += ImportFileToVaultEventHandler;
        this._view.ExportFileFromVaultEvent += ExportFileFromVaultEventHandler;
        this._view.FormClosingEvent += FormClosingEventHandler;
        this._view.SearchFilterAppliedEvent += SearchFilterAppliedEventHandler;
        this._view.OpenSettingsEvent += OpenSettingsEventHandler;
        this._view.SetFilesInVaultListBindingSource(_filesInVaultBindingSource);
        LoadAllFilesInVault();
        ConfigureViewBasedUponAppSettings();
    }

    private void ConfigureViewBasedUponAppSettings()
    {
        if (_appSettings.Mode == ApplicationMode.Advanced)
        {
            _view.SetAdvancedModeView();
        }
        else
        {
            _view.SetBasicModeView();
        }
    }

    private void OpenSettingsEventHandler(object _, EventArgs e)
    {
        _view.PauseView();
        var settingsViewPresenter = _presenterManager.GetSettingsViewPresenter();
        settingsViewPresenter.SettingsConfirmed += (_, _) =>
        {
            _view.ResumeView();
            ConfigureViewBasedUponAppSettings();
        };
    }

    private void AddFileToVaultEventHandler(object sender, EventArgs e)
    {
        string fileToAdd = _fileManager.GetFilePathFromExplorer("All Files (*.*)|*.*");
        if (fileToAdd == null)
        {
            return;
        }

        bool success = _fileManager.AddFileToVault(fileToAdd);
        if (!success)
        {
            ShowMessageBox($"An Error Occurred While Attempting to Encrypt {fileToAdd}");
        }
    }

    private void AddFolderToVaultEventHandler(object sender, EventArgs e)
    {
        var folderToAdd = _fileManager.GetFolderPathFromExplorer();
        if (folderToAdd != null)
        {
            bool success = _fileManager.ZipFolderAndAddToVault(folderToAdd);
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

        string selectedPath = _fileManager.GetFolderPathFromExplorer();
        if (string.IsNullOrWhiteSpace(selectedPath))
        {
            return;
        }

        bool success = _fileManager.DownloadFileFromVault(filePath, selectedPath);

        if (success)
        {
            _fileManager.OpenFolderInExplorer(selectedPath);
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

        _fileManager.OpenFileFromVaultAndReencryptUponClosure(filePath);
    }

    private void DeleteFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var filePath = GetSelectedFilePath();
        if (filePath == null)
        {
            return;
        }
        bool success = _fileManager.DeleteFileFromVault(filePath);
        if (!success)
        {
            _view.ShowFailedToDeleteError();
        }
    }

    private void ExportFileFromVaultEventHandler(object sender, EventArgs e)
    {
        var selectedEncryptedFile = GetSelectedEncryptedFile();
        if (selectedEncryptedFile == null)
        {
            return;
        }

        _presenterManager.GetExportEncryptedFilePresenter(selectedEncryptedFile);
        // TODO Potentially pause view while Export view is open
    }

    private void ImportFileToVaultEventHandler(object sender, EventArgs e)
    {
        _presenterManager.GetImportEncryptedFilePresenter();
        // TODO Potentially pause view while Import view is open
    }

    private void SearchFilterAppliedEventHandler(object sender, EventArgs e)
    {
        string searchValue = _view.SearchValue;
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
        var filesInVault = _filesInVaultBindingSource.List.OfType<FileInformation>();
        filesInVault = filesInVault.Where(x => x.FileName.ToLower().Contains(filter) || x.FileExtension.ToLower().Contains(filter));
        _filesInVaultBindingSource.DataSource = filesInVault;
    }

    private void LoadAllFilesInVault(object o, EventArgs e)
    {
        _filesInVault = _fileManager.GetAllFilesInVault();
        var fileInfo = _filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation);
        var newBindSource = new BindingSource();
        newBindSource.DataSource = fileInfo;
        _filesInVaultBindingSource = newBindSource;
        _view.SetFilesInVaultListBindingSource(_filesInVaultBindingSource);
    }

    private void LoadAllFilesInVault()
    {
        _filesInVault = _fileManager.GetAllFilesInVault();
        var fileInfo = _filesInVault.Where(_ => _.UniquePassword == false && _.DecryptedFileInformation != null)
            .Select(_ => _.DecryptedFileInformation);
        _filesInVaultBindingSource.DataSource = fileInfo;
        ApplySearchFilter(_view.SearchValue);
    }

    private void FormClosingEventHandler(object sender, FormClosingEventArgs e)
    {
        _fileManager.CleanupTempFiles();
    }

    private string GetSelectedFilePath()
    {
        FileInformation fileInVault = _view.SelectedFile;
        if (fileInVault == null)
        {
            return null;
        }

        if (!_filesInVault.Any())
        {
            return null;
        }

        return _filesInVault.First(_ => _.DecryptedFileInformation != null && _.DecryptedFileInformation == fileInVault).FilePath;
    }

    private EncryptedFile GetSelectedEncryptedFile()
    {
        FileInformation fileInVault = _view.SelectedFile;
        if (fileInVault == null)
        {
            return null;
        }

        if (!_filesInVault.Any())
        {
            return null;
        }

        return _filesInVault.First(_ => _.DecryptedFileInformation != null && _.DecryptedFileInformation == fileInVault);
    }

    protected virtual void ShowMessageBox(string message)
    {
        MessageBox.Show(message);
    }
}