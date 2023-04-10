using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;

namespace Application.Managers;

public interface IPresenterManager
{
    /// <summary>
    /// Gets a new instance of the ExportEncryptedFilePresenter with the provided encrypted file and IExportEncryptedFileView.
    /// </summary>
    /// <param name="encryptedFileToExport">The EncryptedFile to be exported.</param>
    /// <param name="exportEncryptedFileView">The optional IExportEncryptedFileView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ExportEncryptedFilePresenter.</returns>
    ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport, IExportEncryptedFileView exportEncryptedFileView = null);

    /// <summary>
    /// Gets a new instance of the ImportEncryptedFilePresenter with the provided or default IImportEncryptedFileView.
    /// </summary>
    /// <param name="importEncryptedFileView">The optional IImportEncryptedFileView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ImportEncryptedFilePresenter.</returns>
    ImportEncryptedFilePresenter GetImportEncryptedFilePresenter(IImportEncryptedFileView importEncryptedFileView = null);

    /// <summary>
    /// Gets a new instance of the SettingsViewPresenter with the provided or default ISettingsView.
    /// </summary>
    /// <param name="settingsView">The optional ISettingsView instance to be used, defaults to null.</param>
    /// <returns>A new instance of SettingsViewPresenter.</returns>
    SettingsViewPresenter GetSettingsViewPresenter(ISettingsView settingsView = null);

    /// <summary>
    /// Gets a new instance of the ChangePasswordViewPresenter with the provided or default IChangePasswordView.
    /// </summary>
    /// <param name="changePasswordView">The optional IChangePasswordView instance to be used, defaults to null.</param>
    /// <returns>A new instance of ChangePasswordViewPresenter.</returns>
    ChangePasswordViewPresenter GetChangePasswordViewManager(IChangePasswordView changePasswordView = null);

    /// <summary>
    /// Gets a new instance of the HomeViewPresenter with the provided or default IHomeView.
    /// </summary>
    /// <param name="homeView">The optional IHomeView instance to be used, defaults to null.</param>
    /// <returns>A new instance of HomeViewPresenter.</returns>
    HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null);

    /// <summary>
    /// Gets a new instance of the WindowsHelloRegisterViewPresenter with the provided or default IWindowsHelloRegisterView.
    /// </summary>
    /// <param name="windowsHelloRegisterView">The optional IWindowsHelloRegisterView instance to be used, defaults to null.</param>
    /// <returns>A new instance of WindowsHelloRegisterViewPresenter.</returns>
    WindowsHelloRegisterViewPresenter GetWindowsHelloRegisterViewPresenter(IWindowsHelloRegisterView windowsHelloRegisterView = null);
}