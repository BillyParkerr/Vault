using Application.Models;
using Application.Presenters;

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
}