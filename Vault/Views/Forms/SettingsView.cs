namespace Application.Views.Forms;

public partial class Settings : Form
{
    public event EventHandler ToggleUserModeEvent;
    public event EventHandler ToggleAuthenticationModeEvent;
    public event EventHandler ChangePasswordEvent;
    public event EventHandler ToggleAutomaticDeletionOfUploadedFilesEvent;
    public event EventHandler ChangeDefaultDownloadLocationEvent;

    public Settings()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    private void AssociateAndRaiseViewEvents()
    {
        BasicModeButton.Click += delegate { ToggleUserModeEvent?.Invoke(this, EventArgs.Empty); };
        AdvancedModeButton.Click += delegate { ToggleUserModeEvent?.Invoke(this, EventArgs.Empty); };
        WindowsHelloModeButton.Click += delegate { ToggleAuthenticationModeEvent?.Invoke(this, EventArgs.Empty); };
        PasswordModeButton.Click += delegate { ToggleAuthenticationModeEvent?.Invoke(this, EventArgs.Empty); };
        DeleteUploadedFilesButton.Click += delegate { ToggleAutomaticDeletionOfUploadedFilesEvent?.Invoke(this, EventArgs.Empty); };
        ChangeDefaultDownloadLocationButton.Click += delegate { ChangeDefaultDownloadLocationEvent?.Invoke(this, EventArgs.Empty); };
        ChangePasswordButton.Click += delegate { ChangePasswordEvent?.Invoke(this, EventArgs.Empty); };
    }
}