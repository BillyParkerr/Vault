using Application.Enums;
using Application.Managers;
using Application.Views.Interfaces;

namespace Application.Presenters;

/// <summary>
/// This presenter is responsible for the handling of the SettingsView.
///
/// Its main focus is to allow the user to decide there chosen settings and commit them
/// to the config file once confirmed.
/// </summary>
public class SettingsViewPresenter
{
    // This event is used to inform any class that subscribes that the user has selected there chosen settings and they
    // have been commited to the AppSettings.json file. This will allow the class to adjust any view it is responsible for.
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
        _appSettings = appSettings;
        _fileManager = fileManager;
        _windowsHelloManager = windowsHelloManager;
        _presenterManager = presenterManager;
        _loginManager = loginManager;

        _view = view;
        _view.ToggleUserModeEvent += ToggleUserModeEventHandler;
        _view.ToggleAuthenticationModeEvent += ToggleAuthenticationModeEventHandler;
        _view.ChangePasswordEvent += ChangePasswordEventHandler;
        _view.ToggleAutomaticDeletionOfUploadedFilesEvent += ToggleAutomaticDeletionOfUploadedFilesEventHandler;
        _view.ChangeDefaultDownloadLocationEvent += ChangeDefaultDownloadLocationEventHandler;
        _view.ConfirmChosenSettingsEvent += ConfirmChosenSettingsEventHandler;
        _view.UserClosedViewEvent += UserClosedViewEventHandler;

        // Take a copy of the current appSettings
        _uncommitedAppSettings = CopyAppSettings(appSettings);
        _ = SetupViewBasedUponAppSettings();

        _view.Show();
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

    /// <summary>
    /// Commits the newly chosen app settings to the AppSettings.json file via the Program.UpdateAppSettings method.
    /// </summary>
    private void CommitAppSettings()
    {
        _appSettings.AuthenticationMethod = _uncommitedAppSettings.AuthenticationMethod;
        _appSettings.DefaultDownloadLocation = _uncommitedAppSettings.DefaultDownloadLocation;
        _appSettings.Mode = _uncommitedAppSettings.Mode;
        _appSettings.DeleteUnencryptedFileUponUpload = _uncommitedAppSettings.DeleteUnencryptedFileUponUpload;
        Program.UpdateAppSettings(_appSettings);
    }

    /// <summary>
    /// This method setups up the SettingsView depending on the existing AppSettings.
    /// So for example if the user was using password mode, the password button would be disabled and
    /// the WindowsHelloModeButton would be activated.
    /// </summary>
    /// <returns></returns>
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

    private async void ConfirmChosenSettingsEventHandler(object _, EventArgs __)
    {
        if (!string.IsNullOrWhiteSpace(GivenNewPassword))
        {
            _loginManager.ChangePassword(GivenNewPassword, GivenOldPassword);
        }

        if (_uncommitedAppSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello
            && _appSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            await WindowsHelloPasswordVerification();
        }

        CommitAppSettings();
        _view.Close();
        SettingsConfirmed?.Invoke(this, EventArgs.Empty);
    }


    private async Task WindowsHelloPasswordVerification()
    {
        // If the user has changed their password we can use this
        if (!string.IsNullOrWhiteSpace(GivenNewPassword))
        {
            _fileManager.ProtectAndSavePassword(GivenNewPassword);
        }
        else
        {
            var verifyPasswordPresenter = _presenterManager.GetVerifyPasswordViewPresenter();

            var passwordVerificationTaskCompletion = new TaskCompletionSource<bool>();
            verifyPasswordPresenter.PasswordVerificationFinished += (_, passwordVerifiedEventArgs) =>
            {
                if (passwordVerifiedEventArgs.PasswordVerified && !string.IsNullOrWhiteSpace(passwordVerifiedEventArgs.GivenPassword))
                {
                    _fileManager.ProtectAndSavePassword(passwordVerifiedEventArgs.GivenPassword);
                    passwordVerificationTaskCompletion.SetResult(true);
                }
                else
                {
                    _uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.Password;
                    passwordVerificationTaskCompletion.SetResult(false);
                }
            };

            await passwordVerificationTaskCompletion.Task;
        }
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

    private async void ToggleAuthenticationModeEventHandler(object _, EventArgs __)
    {
        if (_uncommitedAppSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            if (_appSettings.AuthenticationMethod == AuthenticationMethod.Password)
            {
                bool authenticated = await _windowsHelloManager.AuthenticateWithWindowsHelloAsync("Please authenticate to use windows hello");
                if (authenticated)
                {
                    _uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
                    _view.DisableWindowsHelloModeButton();
                    _view.EnablePasswordModeButton();
                }
                else
                {
                    _view.ShowMessageBox("Failed to authenticate with Windows Hello.Vault will remain using password authentication.");
                    return;
                }
            }
            else
            {
                _uncommitedAppSettings.AuthenticationMethod = AuthenticationMethod.WindowsHello;
                _view.DisableWindowsHelloModeButton();
                _view.EnablePasswordModeButton();
            }
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
        var newDefaultLocation = _fileManager.GetFolderPathFromExplorer("Select default download location");
        _uncommitedAppSettings.DefaultDownloadLocation = newDefaultLocation;
    }
}