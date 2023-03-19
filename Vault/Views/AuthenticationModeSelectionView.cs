namespace Application.Views;

public partial class AuthenticationModeSelectionView : Form, IAuthenticationModeSelectionView
{
    public event EventHandler PasswordModeSelected;
    public event EventHandler WindowsHelloModeSelected;

    public AuthenticationModeSelectionView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    private void AssociateAndRaiseViewEvents()
    {
        PasswordModeButton.Click += delegate { PasswordModeSelected?.Invoke(this, EventArgs.Empty); };
        WindowsHelloModeButton.Click += delegate { WindowsHelloModeSelected?.Invoke(this, EventArgs.Empty); };
    }
}