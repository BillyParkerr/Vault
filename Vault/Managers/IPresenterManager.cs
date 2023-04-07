using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;

namespace Application.Managers;

public interface IPresenterManager
{
    LoginViewPresenter GetLoginViewPresenter(ILoginView loginView = null);
    AuthenticationModeSelectionViewPresenter GetAuthenticationModeSelectionViewPresenter(IAuthenticationModeSelectionView authenticationModeSelectionView = null);
    ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport, IExportEncryptedFileView exportEncryptedFileView = null);
    ImportEncryptedFilePresenter GetImportEncryptedFilePresenter(IImportEncryptedFileView importEncryptedFileView = null);
    RegistrationViewPresenter GetRegistrationViewPresenter(IRegisterView registerView = null);
    SettingsViewPresenter GetSettingsViewPresenter(ISettingsView settingsView = null);
    ChangePasswordViewPresenter GetChangePasswordViewManager(IChangePasswordView changePasswordView = null);
    HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null);
}