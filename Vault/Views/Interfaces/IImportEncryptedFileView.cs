namespace Application.Views.Interfaces;

public interface IImportEncryptedFileView
{
    // Properties
    string GivenPassword { get; }

    // Event
    event EventHandler ConfirmEvent;

    // Methods
    void Show();
    void Close();
    void ShowBlankPasswordError();
}