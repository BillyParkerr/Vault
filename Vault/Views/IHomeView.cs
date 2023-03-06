using Application.Models;

namespace Application.Views;

public interface IHomeView
{
    // Properties
    FileInformation? SelectedFile { get; }

    // Events
    event EventHandler AddFileToVaultEvent;
    event EventHandler AddFolderToVaultEvent;
    event EventHandler DownloadFileFromVaultEvent;
    event EventHandler DeleteFileFromVaultEvent;
    event EventHandler OpenFileFromVaultEvent;
    event EventHandler ExportFileFromVaultEvent;
    event EventHandler ImportFileToVaultEvent;
    event FormClosingEventHandler FormClosingEvent;

    // Methods
    void SetFilesInVaultListBindingSource(BindingSource filesInVaultList);
    void Show();
    void ShowFailedToDeleteError();
}