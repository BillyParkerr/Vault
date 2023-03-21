namespace Application.Views.Interfaces;

public interface ISettingsView
{
    public event EventHandler ToggleUserModeEvent;
    public event EventHandler ToggleAuthenticationModeEvent;
    public event EventHandler ChangePasswordEvent;
    public event EventHandler ToggleAutomaticDeletionOfUploadedFilesEvent;
    public event EventHandler ChangeDefaultDownloadLocationEvent;
}