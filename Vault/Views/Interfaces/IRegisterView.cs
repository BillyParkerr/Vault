namespace Application.Views.Interfaces;

public interface IRegisterView
{
    // Properties
    string GivenPassword { get; }
    string GivenSecondPassword { get; }


    // Events
    event EventHandler RegisterEvent;

    // Methods
    public void ShowBlankPasswordError();
    public void ShowPasswordMismatchError();
    public void ShowPasswordTooShortError();

    void Show();
    void Close();
}