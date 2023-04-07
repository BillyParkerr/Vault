using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class SettingsViewPresenter
{
    public virtual event EventHandler SettingsConfirmed;

    private readonly ISettingsView view;
    private readonly IFileManager fileManager;
    private readonly IWindowsHelloManager windowsHelloManager;
    private readonly IPresenterManager presenterManager;
    private readonly ILoginManager loginManager;
    private readonly AppSettings appSettings;

    private protected string givenNewPassword;
    private protected string givenOldPassword;

    private readonly AppSettings uncommitedAppSettings;

    public SettingsViewPresenter(ISettingsView view, IFileManager fileManager, AppSettings appSettings, 
        IWindowsHelloManager windowsHelloManager, IPresenterManager presenterManager, ILoginManager loginManager)
    {
        this.appSettings = appSettings;
        this.fileManager = fileManager;
        this.windowsHelloManager = windowsHelloManager;
        this.presenterManager = presenterManager;
        this.loginManager = loginManager;

        this.view = view;
        this.view.ToggleUserModeEvent += ToggleUserModeEventHandler;
        this.view.ToggleAuthenticationModeEvent += ToggleAuthenticationModeEventHandler;
        this.view.ChangePasswordEvent += ChangePasswordEventHandler;
        this.view.ToggleAutomaticDeletionOfUploadedFilesEvent += ToggleAutomaticDeletionOfUploadedFilesEventHandler;
        this.view.ChangeDefaultDownloadLocationEvent += ChangeDefaultDownloadLocationEventHandler;
        this.view.ConfirmChosenSettingsEvent += ConfirmChosenSettingsEventHandler;
        this.view.UserClosedViewEvent += UserClosedViewEventHandler;

        // Take a copy of the current appSettings
        uncommitedAppSettings = CopyAppSettings(appSettings);
        _ = SetupViewBasedUponAppSettings();

        this.view.Show();
    }

    private static AppSettings CopyAppSettings(AppSettings settings)
    {
        AppSettings newSettings = new()
        {
            AuthenticationMethod = settings.AuthenticationMethod,
            DeleteUnencryptedFileUponUpload = settings.DeleteUnencryptedFileUponUpload,
            Mode = settings.Mode,
            DefaultDownloadLocation = settings.DefaultDownloadLocation
        };

        return newSettings;
    }

    private void CommitAppSettings()
    {
        appSettings.AuthenticationMethod = uncommitedAppSettings.AuthenticationMethod;
        appSettings.DefaultDownloadLocation = uncommitedAppSettings.DefaultDownloadLocation;
        appSettings.Mode = uncommitedAppSettings.Mode;
        appSettings.DeleteUnencryptedFileUponUpload = uncommitedAppSettings.DeleteUnencryptedFileUponUpload;
        Program.UpdateAppSettings(appSettings);
    }

    private async Task SetupViewBasedUponAppSettings()
    {
        // Authentication Method.
        if (appSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            view.DisablePasswordModeButton();
        }
        else if (appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
        {
            view.DisableWindowsHelloModeButton();
        }

        // This will ensure that if windows hello is not availabe the user will not be able to change authentication mode.
        if (await windowsHelloManager.IsWindowsHelloAvailable() == false)
        {
            view.DisableWindowsHelloModeButton();
            view.DisablePasswordModeButton();
        }

        // Application Mode.
        if (appSettings.Mode == ApplicationMode.Basic)
        {
            view.DisableBasicModeButton();
        }
        else if (appSettings.Mode == ApplicationMode.Advanced)
        {
            view.DisableAdvancedModeButton();
        }

        // Deletion of base unencrypted files upon uploading to the vault.
        if (appSettings.DeleteUnencryptedFileUponUpload == true)
        {
            view.SetDeletionOfUploadedFilesToYes();
        }
        else
        {
            view.SetDeletionOfUploadedFilesToNo();
        }
    }

    private void UserClosedViewEventHandler(object _, EventArgs __)
    {
        SettingsConfirmed?.Invoke(this, EventArgs.Empty);
    }

    private void ConfirmChosenSettingsEventHandler(object _, EventArgs __)
    {
        CommitAppSettings();
        if (!string.IsNullOrWhiteSpace(givenNewPassword))
        {
            loginManager.ChangePassword(givenNewPassword, givenOldPassword);
        }
        view.Close();
        SettingsConfirmed?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleUserModeEventHandler(object _, EventArgs __)
    {
        if (uncommitedAppSettings.Mode == ApplicationMode.Basic)
        {
            uncommitedAppSettings.Mode = ApplicationMode.Advanced;
            view.DisableAdvancedModeButton();
            view.EnableBasicModeButton();
        }
        else
        {
            uncommitedAppSettings.Mode = ApplicationMode.Basic;
            view.DisableBasicModeButton();
            view.EnableAdvancedModeButton();
        }
    }

    private void ToggleAuthenticationModeEventHandler(object _, EventArgs __)
    {
        if (uncommitedAppSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
            view.DisableWindowsHelloModeButton();
            view.EnablePasswordModeButton();
        }
        else
        {
            uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.Password;
            view.EnableWindowsHelloModeButton();
            view.DisablePasswordModeButton();
        }
    }

    private void ChangePasswordEventHandler(object _, EventArgs __)
    {
        var changePasswordViewPresenter = presenterManager.GetChangePasswordViewManager();
        changePasswordViewPresenter.NewPasswordChosen += (_, passwordChangedArguments) =>
        {
            givenNewPassword = passwordChangedArguments.EnteredNewPassword;
            givenOldPassword = passwordChangedArguments.EnteredOldPassword;
        };
    }

    private void ToggleAutomaticDeletionOfUploadedFilesEventHandler(object _, EventArgs __)
    {
        if (uncommitedAppSettings.DeleteUnencryptedFileUponUpload == false)
        {
            uncommitedAppSettings.DeleteUnencryptedFileUponUpload = true;
            view.SetDeletionOfUploadedFilesToYes();
        }
        else
        {
            uncommitedAppSettings.DeleteUnencryptedFileUponUpload = false;
            view.SetDeletionOfUploadedFilesToNo();
        }
    }

    private void ChangeDefaultDownloadLocationEventHandler(object _, EventArgs __)
    {
        var newDefaultLocation = fileManager.GetFolderPathFromExplorer();
        uncommitedAppSettings.DefaultDownloadLocation = newDefaultLocation;
    }
}