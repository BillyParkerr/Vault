namespace Application.Views.Interfaces;

public interface IVerifyPasswordView
{
    // Properties
    string GivenPassword { get; }

    // Event
    event EventHandler VerifyEvent;
    event EventHandler UserClosedFormEvent;

    // Methods
    void Show();
    void Close();
    void ShowBlankPasswordGivenError();
    void ShowIncorrectPasswordError();
}