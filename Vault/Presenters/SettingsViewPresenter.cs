using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

public class SettingsViewPresenter
{
    public virtual event EventHandler SettingsConfirmed;

    private readonly ISettingsView _view;
    private readonly IFileManager _fileManager;
    private readonly IWindowsHelloManager _windowsHelloManager;
    private readonly IPresenterManager _presenterManager;
    private readonly ILoginManager _loginManager;
    private readonly AppSettings _appSettings;

    private protected string GivenNewPassword;
    private protected string GivenOldPassword;

    private readonly AppSettings _uncommitedAppSettings;

    public SettingsViewPresenter(ISettingsView view, IFileManager fileManager, AppSettings appSettings, 
        IWindowsHelloManager windowsHelloManager, IPresenterManager presenterManager, ILoginManager loginManager)
    {
        this._appSettings = appSettings;
        this._fileManager = fileManager;
        this._windowsHelloManager = windowsHelloManager;
        this._presenterManager = presenterManager;
        this._loginManager = loginManager;

        this._view = view;
        this._view.ToggleUserModeEvent += ToggleUserModeEventHandler;
        this._view.ToggleAuthenticationModeEvent += ToggleAuthenticationModeEventHandler;
        this._view.ChangePasswordEvent += ChangePasswordEventHandler;
        this._view.ToggleAutomaticDeletionOfUploadedFilesEvent += ToggleAutomaticDeletionOfUploadedFilesEventHandler;
        this._view.ChangeDefaultDownloadLocationEvent += ChangeDefaultDownloadLocationEventHandler;
        this._view.ConfirmChosenSettingsEvent += ConfirmChosenSettingsEventHandler;
        this._view.UserClosedViewEvent += UserClosedViewEventHandler;

        // Take a copy of the current appSettings
        _uncommitedAppSettings = CopyAppSettings(appSettings);
        _ = SetupViewBasedUponAppSettings();

        this._view.Show();
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
        _appSettings.AuthenticationMethod = _uncommitedAppSettings.AuthenticationMethod;
        _appSettings.DefaultDownloadLocation = _uncommitedAppSettings.DefaultDownloadLocation;
        _appSettings.Mode = _uncommitedAppSettings.Mode;
        _appSettings.DeleteUnencryptedFileUponUpload = _uncommitedAppSettings.DeleteUnencryptedFileUponUpload;
        Program.UpdateAppSettings(_appSettings);
    }

    private async Task SetupViewBasedUponAppSettings()
    {
        // Authentication Method.
        if (_appSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            _view.DisablePasswordModeButton();
        }
        else if (_appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
        {
            _view.DisableWindowsHelloModeButton();
        }

        // This will ensure that if windows hello is not availabe the user will not be able to change authentication mode.
        if (await _windowsHelloManager.IsWindowsHelloAvailable() == false)
        {
            _view.DisableWindowsHelloModeButton();
            _view.DisablePasswordModeButton();
        }

        // Application Mode.
        if (_appSettings.Mode == ApplicationMode.Basic)
        {
            _view.DisableBasicModeButton();
        }
        else if (_appSettings.Mode == ApplicationMode.Advanced)
        {
            _view.DisableAdvancedModeButton();
        }

        // Deletion of base unencrypted files upon uploading to the vault.
        if (_appSettings.DeleteUnencryptedFileUponUpload == true)
        {
            _view.SetDeletionOfUploadedFilesToYes();
        }
        else
        {
            _view.SetDeletionOfUploadedFilesToNo();
        }
    }

    private void UserClosedViewEventHandler(object _, EventArgs __)
    {
        SettingsConfirmed?.Invoke(this, EventArgs.Empty);
    }

    private void ConfirmChosenSettingsEventHandler(object _, EventArgs __)
    {
        CommitAppSettings();
        if (!string.IsNullOrWhiteSpace(GivenNewPassword))
        {
            _loginManager.ChangePassword(GivenNewPassword, GivenOldPassword);
        }
        _view.Close();
        SettingsConfirmed?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleUserModeEventHandler(object _, EventArgs __)
    {
        if (_uncommitedAppSettings.Mode == ApplicationMode.Basic)
        {
            _uncommitedAppSettings.Mode = ApplicationMode.Advanced;
            _view.DisableAdvancedModeButton();
            _view.EnableBasicModeButton();
        }
        else
        {
            _uncommitedAppSettings.Mode = ApplicationMode.Basic;
            _view.DisableBasicModeButton();
            _view.EnableAdvancedModeButton();
        }
    }

    private void ToggleAuthenticationModeEventHandler(object _, EventArgs __)
    {
        if (_uncommitedAppSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            _uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
            _view.DisableWindowsHelloModeButton();
            _view.EnablePasswordModeButton();
        }
        else
        {
            _uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.Password;
            _view.EnableWindowsHelloModeButton();
            _view.DisablePasswordModeButton();
        }
    }

    private void ChangePasswordEventHandler(object _, EventArgs __)
    {
        var changePasswordViewPresenter = _presenterManager.GetChangePasswordViewManager();
        changePasswordViewPresenter.NewPasswordChosen += (_, passwordChangedArguments) =>
        {
            GivenNewPassword = passwordChangedArguments.EnteredNewPassword;
            GivenOldPassword = passwordChangedArguments.EnteredOldPassword;
        };
    }

    private void ToggleAutomaticDeletionOfUploadedFilesEventHandler(object _, EventArgs __)
    {
        if (_uncommitedAppSettings.DeleteUnencryptedFileUponUpload == false)
        {
            _uncommitedAppSettings.DeleteUnencryptedFileUponUpload = true;
            _view.SetDeletionOfUploadedFilesToYes();
        }
        else
        {
            _uncommitedAppSettings.DeleteUnencryptedFileUponUpload = false;
            _view.SetDeletionOfUploadedFilesToNo();
        }
    }

    private void ChangeDefaultDownloadLocationEventHandler(object _, EventArgs __)
    {
        var newDefaultLocation = _fileManager.GetFolderPathFromExplorer();
        _uncommitedAppSettings.DefaultDownloadLocation = newDefaultLocation;
    }
}