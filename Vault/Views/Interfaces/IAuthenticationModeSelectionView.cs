namespace Application.Views.Interfaces;

public interface IAuthenticationModeSelectionView
{
    event EventHandler PasswordModeSelected;
    event EventHandler WindowsHelloModeSelected;

    void Close();
}