namespace Application.Views.Interfaces;

public interface ISettingsView
{
    event EventHandler ToggleUserModeEvent;
    event EventHandler ToggleAuthenticationModeEvent;
    event EventHandler ChangePasswordEvent;
    event EventHandler ToggleAutomaticDeletionOfUploadedFilesEvent;
    event EventHandler ChangeDefaultDownloadLocationEvent;
    event EventHandler ConfirmChosenSettingsEvent;
    event EventHandler UserClosedViewEvent;

    void EnableAdvancedModeButton();
    void DisableAdvancedModeButton();
    void EnableBasicModeButton();
    void DisableBasicModeButton();
    void EnableWindowsHelloModeButton();
    void DisableWindowsHelloModeButton();
    void EnablePasswordModeButton();
    void DisablePasswordModeButton();
    void SetDeletionOfUploadedFilesToYes();
    void SetDeletionOfUploadedFilesToNo();
    void Show();
    void Close();
}