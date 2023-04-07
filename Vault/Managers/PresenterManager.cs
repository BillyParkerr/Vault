using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using SimpleInjector;

namespace Application.Managers;

public class PresenterManager: IPresenterManager
{
    private readonly Container container = Program.container;

    private IDatabaseManager databaseManager;
    private readonly IEncryptionManager encryptionManager;
    private readonly IFileManager fileManager;
    private readonly ILoginManager loginManager;
    private readonly IWindowsHelloManager windowsHelloManager;
    private readonly AppSettings appSettings;

    public PresenterManager(IDatabaseManager databaseManager, 
        IEncryptionManager encryptionManager, 
        IFileManager fileManager, 
        ILoginManager loginManager,
        IWindowsHelloManager windowsHelloManager,
        AppSettings appSettings)
    {
        this.databaseManager = databaseManager;
        this.encryptionManager = encryptionManager;
        this.fileManager = fileManager;
        this.loginManager = loginManager;
        this.appSettings = appSettings;
        this.windowsHelloManager = windowsHelloManager;
    }

    public HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null)
    {
        homeView ??= container.GetInstance<IHomeView>();

        return new HomeViewPresenter(homeView, fileManager, databaseManager, this, appSettings);
    }

    public LoginViewPresenter GetLoginViewPresenter(ILoginView loginView = null)
    {
        loginView ??= container.GetInstance<ILoginView>();
        return new(loginView, loginManager, encryptionManager);
    }

    public AuthenticationModeSelectionViewPresenter GetAuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView authenticationModeSelectionView = null)
    {
        authenticationModeSelectionView ??= container.GetInstance<IAuthenticationModeSelectionView>();
        return new(authenticationModeSelectionView, appSettings);
    }

    public ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport, IExportEncryptedFileView exportEncryptedFileView = null)
    {
        exportEncryptedFileView ??= container.GetInstance<IExportEncryptedFileView>();
        return new(exportEncryptedFileView, fileManager, encryptedFileToExport);
    }

    public ImportEncryptedFilePresenter GetImportEncryptedFilePresenter(IImportEncryptedFileView importEncryptedFileView = null)
    {
        importEncryptedFileView ??= container.GetInstance<IImportEncryptedFileView>();
        return new(importEncryptedFileView, fileManager);
    }

    public RegistrationViewPresenter GetRegistrationViewPresenter(IRegisterView registerView = null)
    {
        registerView ??= container.GetInstance<IRegisterView>();
        return new(loginManager, registerView);
    }

    public SettingsViewPresenter GetSettingsViewPresenter(ISettingsView settingsView = null)
    {
        settingsView ??= container.GetInstance<ISettingsView>();
        return new(settingsView, fileManager, appSettings, windowsHelloManager, this, loginManager);
    }

    public ChangePasswordViewPresenter GetChangePasswordViewManager(IChangePasswordView changePasswordView = null)
    {
        changePasswordView ??= container.GetInstance<IChangePasswordView>();
        return new(loginManager, changePasswordView);
    }
}