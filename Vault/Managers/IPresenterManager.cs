using Application.Models;
using Application.Presenters;
using Application.Views.Interfaces;

namespace Application.Managers;

public interface IPresenterManager
{
    LoginViewPresenter GetLoginViewPresenter();
    AuthenticationModeSelectionViewPresenter GetAuthenticationModeSelectionViewPresenter();
    ExportEncryptedFilePresenter GetExportEncryptedFilePresenter(EncryptedFile encryptedFileToExport);
    ImportEncryptedFilePresenter GetImportEncryptedFilePresenter();
    RegistrationViewPresenter GetRegistrationViewPresenter();
    SettingsViewPresenter GetSettingsViewPresenter();
    ChangePasswordViewPresenter GetChangePasswordViewManager();
    HomeViewPresenter GetHomeViewPresenter(IHomeView homeView = null);
}