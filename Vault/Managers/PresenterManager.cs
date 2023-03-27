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

    // TODO Add HomeViewPresenter and take HomeView as a parameter so that this can be used in Program.cs

    public LoginViewPresenter GetLoginViewPresenter()
    {
        ILoginView loginView = container.GetInstance<ILoginView>();
        return new(loginView, loginManager, encryptionManager);
    }

    public AuthenticationModeSelectionViewPresenter GetAuthenticationModeSelectionViewPresenter()
    {
        IAuthenticationModeSelectionView authenticationModeSelectionView = container.GetInstance<IAuthenticationModeSelectionView>();
        return new(authenticationModeSelectionView, appSettings);
    }

    public ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport)
    {
        IExportEncryptedFileView exportEncryptedFileView = container.GetInstance<IExportEncryptedFileView>();
        return new(exportEncryptedFileView, fileManager, encryptedFileToExport);
    }

    public ImportEncryptedFilePresenter GetImportEncryptedFilePresenter()
    {
        IImportEncryptedFileView importEncryptedFileView = container.GetInstance<IImportEncryptedFileView>();
        return new(importEncryptedFileView, fileManager);
    }

    public RegistrationViewPresenter GetRegistrationViewPresenter()
    {
        IRegisterView registerView = container.GetInstance<IRegisterView>();
        return new(loginManager, registerView);
    }

    public SettingsViewPresenter GetSettingsViewPresenter()
    {
        ISettingsView settingsView = container.GetInstance<ISettingsView>();
        return new(settingsView, fileManager, appSettings, windowsHelloManager, this, loginManager);
    }

    public ChangePasswordViewPresenter GetChangePasswordViewManager()
    {
        IChangePasswordView changePasswordView = container.GetInstance<IChangePasswordView>();
        return new(loginManager, changePasswordView);
    }
}