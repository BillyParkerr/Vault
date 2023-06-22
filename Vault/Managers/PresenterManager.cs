using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;
using SimpleInjector;

namespace Application.Managers;

/// <summary>
/// Class responsible for the generation of presenters.
///
/// The main benefit of having this class is that it allows classes that require other presenters to be created to Mock the required presenter.
/// This allows for easier and more isolated unit testing and ensures classes conform to SOLID principles.
/// </summary>
public class PresenterManager: IPresenterManager
{
    private readonly Container _container = Program.Container;

    private readonly IDatabaseManager _databaseManager;
    private readonly IFileManager _fileManager;
    private readonly ILoginManager _loginManager;
    private readonly IWindowsHelloManager _windowsHelloManager;
    private readonly AppSettings _appSettings;

    public PresenterManager(IDatabaseManager databaseManager, 
        IFileManager fileManager, 
        ILoginManager loginManager,
        IWindowsHelloManager windowsHelloManager,
        AppSettings appSettings)
    {
        _databaseManager = databaseManager;
        _fileManager = fileManager;
        _loginManager = loginManager;
        _appSettings = appSettings;
        _windowsHelloManager = windowsHelloManager;
    }

    /// <summary>
    /// Gets a new instance of the HomeViewPresenter with the provided or default IHomeView.
    /// </summary>
    /// <param name="homeView">The optional IHomeView instance to be used, defaults to null.</param>
    /// <returns>A new instance of HomeViewPresenter.</returns>
    public HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null)
    {
        homeView ??= _container.GetInstance<IHomeView>();

        return new HomeViewPresenter(homeView, _fileManager, _databaseManager, this, _appSettings);
    }

    /// <summary>
    /// Gets a new instance of the ExportEncryptedFilePresenter with the provided encrypted file and IExportEncryptedFileView.
    /// </summary>
    /// <param name="encryptedFileToExport">The EncryptedFile to be exported.</param>
    /// <param name="exportEncryptedFileView">The optional IExportEncryptedFileView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ExportEncryptedFilePresenter.</returns>
    public ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport, IExportEncryptedFileView exportEncryptedFileView = null)
    {
        exportEncryptedFileView ??= _container.GetInstance<IExportEncryptedFileView>();
        return new ExportEncryptedFilePresenter(exportEncryptedFileView, _fileManager, encryptedFileToExport);
    }

    /// <summary>
    /// Gets a new instance of the ImportEncryptedFilePresenter with the provided or default IImportEncryptedFileView.
    /// </summary>
    /// <param name="importEncryptedFileView">The optional IImportEncryptedFileView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ImportEncryptedFilePresenter.</returns>
    public ImportEncryptedFilePresenter GetImportEncryptedFilePresenter(IImportEncryptedFileView importEncryptedFileView = null)
    {
        importEncryptedFileView ??= _container.GetInstance<IImportEncryptedFileView>();
        return new ImportEncryptedFilePresenter(importEncryptedFileView, _fileManager);
    }

    /// <summary>
    /// Gets a new instance of the SettingsViewPresenter with the provided or default ISettingsView.
    /// </summary>
    /// <param name="settingsView">The optional ISettingsView instance to be used, defaults to null.</param>
    /// <returns>A new instance of SettingsViewPresenter.</returns>
    public SettingsViewPresenter GetSettingsViewPresenter(ISettingsView settingsView = null)
    {
        settingsView ??= _container.GetInstance<ISettingsView>();
        return new SettingsViewPresenter(settingsView, _fileManager, _appSettings, _windowsHelloManager, this, _loginManager);
    }

    /// <summary>
    /// Gets a new instance of the ChangePasswordViewPresenter with the provided or default IChangePasswordView.
    /// </summary>
    /// <param name="changePasswordView">The optional IChangePasswordView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ChangePasswordViewPresenter.</returns>
    public ChangePasswordViewPresenter GetChangePasswordViewManager(IChangePasswordView changePasswordView = null)
    {
        changePasswordView ??= _container.GetInstance<IChangePasswordView>();
        return new ChangePasswordViewPresenter(_loginManager, changePasswordView);
    }

    /// <summary>
    /// Gets a new instance of the WindowsHelloRegisterViewPresenter with the provided or default IWindowsHelloRegisterView.
    /// </summary>
    /// <param name="windowsHelloRegisterView">The optional IWindowsHelloRegisterView instance to be used, defaults to null.</param>
    /// <returns>A new instance of WindowsHelloRegisterViewPresenter.</returns>
    public WindowsHelloRegisterViewPresenter GetWindowsHelloRegisterViewPresenter(
        IWindowsHelloRegisterView windowsHelloRegisterView = null)
    {
        windowsHelloRegisterView ??= _container.GetInstance<IWindowsHelloRegisterView>();
        return new WindowsHelloRegisterViewPresenter(windowsHelloRegisterView, _windowsHelloManager, _loginManager);
    }

    public VerifyPasswordViewPresenter GetVerifyPasswordViewPresenter(IVerifyPasswordView verifyPasswordView = null)
    {
        verifyPasswordView ??= _container.GetInstance<IVerifyPasswordView>();
        return new VerifyPasswordViewPresenter(verifyPasswordView, _loginManager);
    }
}