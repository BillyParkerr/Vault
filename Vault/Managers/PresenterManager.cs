using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using SimpleInjector;

namespace Application.Managers;

public class PresenterManager: IPresenterManager
{
    private readonly Container _container = Program.Container;

    private readonly IDatabaseManager _databaseManager;
    private readonly IEncryptionManager _encryptionManager;
    private readonly IFileManager _fileManager;
    private readonly ILoginManager _loginManager;
    private readonly IWindowsHelloManager _windowsHelloManager;
    private readonly AppSettings _appSettings;

    public PresenterManager(IDatabaseManager databaseManager, 
        IEncryptionManager encryptionManager, 
        IFileManager fileManager, 
        ILoginManager loginManager,
        IWindowsHelloManager windowsHelloManager,
        AppSettings appSettings)
    {
        this._databaseManager = databaseManager;
        this._encryptionManager = encryptionManager;
        this._fileManager = fileManager;
        this._loginManager = loginManager;
        this._appSettings = appSettings;
        this._windowsHelloManager = windowsHelloManager;
    }

    public HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null)
    {
        homeView ??= _container.GetInstance<IHomeView>();

        return new HomeViewPresenter(homeView, _fileManager, _databaseManager, this, _appSettings);
    }

    public LoginViewPresenter GetLoginViewPresenter(ILoginView loginView = null)
    {
        loginView ??= _container.GetInstance<ILoginView>();
        return new LoginViewPresenter(loginView, _loginManager, _encryptionManager);
    }

    public AuthenticationModeSelectionViewPresenter GetAuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView authenticationModeSelectionView = null)
    {
        authenticationModeSelectionView ??= _container.GetInstance<IAuthenticationModeSelectionView>();
        return new AuthenticationModeSelectionViewPresenter(authenticationModeSelectionView, _appSettings);
    }

    public ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport, IExportEncryptedFileView exportEncryptedFileView = null)
    {
        exportEncryptedFileView ??= _container.GetInstance<IExportEncryptedFileView>();
        return new ExportEncryptedFilePresenter(exportEncryptedFileView, _fileManager, encryptedFileToExport);
    }

    public ImportEncryptedFilePresenter GetImportEncryptedFilePresenter(IImportEncryptedFileView importEncryptedFileView = null)
    {
        importEncryptedFileView ??= _container.GetInstance<IImportEncryptedFileView>();
        return new ImportEncryptedFilePresenter(importEncryptedFileView, _fileManager);
    }

    public RegistrationViewPresenter GetRegistrationViewPresenter(IRegisterView registerView = null)
    {
        registerView ??= _container.GetInstance<IRegisterView>();
        return new RegistrationViewPresenter(_loginManager, registerView);
    }

    public SettingsViewPresenter GetSettingsViewPresenter(ISettingsView settingsView = null)
    {
        settingsView ??= _container.GetInstance<ISettingsView>();
        return new SettingsViewPresenter(settingsView, _fileManager, _appSettings, _windowsHelloManager, this, _loginManager);
    }

    public ChangePasswordViewPresenter GetChangePasswordViewManager(IChangePasswordView changePasswordView = null)
    {
        changePasswordView ??= _container.GetInstance<IChangePasswordView>();
        return new ChangePasswordViewPresenter(_loginManager, changePasswordView);
    }

    public WindowsHelloRegisterViewPresenter GetWindowsHelloRegisterViewPresenter(
        IWindowsHelloRegisterView windowsHelloRegisterView = null)
    {
        windowsHelloRegisterView ??= _container.GetInstance<IWindowsHelloRegisterView>();
        return new WindowsHelloRegisterViewPresenter(windowsHelloRegisterView, _windowsHelloManager, _loginManager);
    }
}