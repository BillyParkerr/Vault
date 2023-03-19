namespace Application.Views;

public interface IAuthenticationModeSelectionView
{
    event EventHandler PasswordModeSelected;
    event EventHandler WindowsHelloModeSelected;

    void Close();
}