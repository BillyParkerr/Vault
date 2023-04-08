namespace Application.Views.Interfaces;

public interface IChangePasswordView
{
    // Properties
    string GivenOldPassword { get; }
    string GivenNewPassword { get; }
    string GivenSecondNewPassword { get; }


    // Events
    event EventHandler ConfirmPasswordEvent;

    // Methods
    public void ShowIncorrectOldPasswordError();
    public void ShowBlankNewPasswordError();
    public void ShowBlankOldPasswordError();
    public void ShowPasswordMismatchError();
    public void ShowNewPasswordTooShortError();


    void Show();
    void Close();
}