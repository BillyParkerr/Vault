namespace Application.Views;

public interface IWindowsHelloRegisterView
{
    // Properties
    string GivenPassword { get; }
    string GivenSecondPassword { get; }


    // Events
    event EventHandler ConfirmEvent;

    // Methods
    public void ShowBlankPasswordError();
    public void ShowPasswordMismatchError();
    public void ShowPasswordTooShortError();

    void Show();
    void Close();
}