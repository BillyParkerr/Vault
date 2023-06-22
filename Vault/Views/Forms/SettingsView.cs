using Application.Views.Interfaces;

namespace Application.Views.Forms;

public partial class SettingsView : Form, ISettingsView
{
    public event EventHandler ToggleUserModeEvent;
    public event EventHandler ToggleAuthenticationModeEvent;
    public event EventHandler ChangePasswordEvent;
    public event EventHandler ToggleAutomaticDeletionOfUploadedFilesEvent;
    public event EventHandler ChangeDefaultDownloadLocationEvent;
    public event EventHandler ConfirmChosenSettingsEvent;
    public event EventHandler UserClosedViewEvent;

    private bool _confirmedButtonPressed = false;

    public SettingsView()
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
        ConfirmSettingsButton.Click += delegate
        {
            _confirmedButtonPressed = true;
            ConfirmChosenSettingsEvent?.Invoke(this, EventArgs.Empty);
        };
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_confirmedButtonPressed)
        {
            DialogResult result = MessageBox.Show("Any non confirmed setting changes will be lost. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                UserClosedViewEvent.Invoke(this, EventArgs.Empty);
            }
        }
        base.OnFormClosing(e);
    }

    public void EnableBasicModeButton()
    {
        BasicModeButton.Enabled = true;
        BasicModeButton.BackColor = SystemColors.Control; // Restore the default background color
        BasicModeButton.ForeColor = SystemColors.ControlText; // Restore the default text color
    }

    public void DisableBasicModeButton()
    {
        BasicModeButton.Enabled = false;
        BasicModeButton.BackColor = Color.LightGray; // Change the background color to light gray
        BasicModeButton.ForeColor = Color.DarkGray; // Change the text color to dark gray

    }

    public void EnableAdvancedModeButton()
    {
        AdvancedModeButton.Enabled = true;
        AdvancedModeButton.BackColor = SystemColors.Control; // Restore the default background color
        AdvancedModeButton.ForeColor = SystemColors.ControlText; // Restore the default text color
    }

    public void DisableAdvancedModeButton()
    {
        AdvancedModeButton.Enabled = false;
        AdvancedModeButton.BackColor = Color.LightGray; // Change the background color to light gray
        AdvancedModeButton.ForeColor = Color.DarkGray; // Change the text color to dark gray
    }

    public void EnableWindowsHelloModeButton()
    {
        WindowsHelloModeButton.Enabled = true;
        WindowsHelloModeButton.BackColor = SystemColors.Control; // Restore the default background color
        WindowsHelloModeButton.ForeColor = SystemColors.ControlText; // Restore the default text color
    }

    public void DisableWindowsHelloModeButton()
    {
        WindowsHelloModeButton.Enabled = false;
        WindowsHelloModeButton.BackColor = Color.LightGray; // Change the background color to light gray
        WindowsHelloModeButton.ForeColor = Color.DarkGray; // Change the text color to dark gray
    }

    public void EnablePasswordModeButton()
    {
        PasswordModeButton.Enabled = true;
        PasswordModeButton.BackColor = SystemColors.Control; // Restore the default background color
        PasswordModeButton.ForeColor = SystemColors.ControlText; // Restore the default text color
    }

    public void DisablePasswordModeButton()
    {
        PasswordModeButton.Enabled = false;
        PasswordModeButton.BackColor = Color.LightGray; // Change the background color to light gray
        PasswordModeButton.ForeColor = Color.DarkGray; // Change the text color to dark gray
    }

    public void SetDeletionOfUploadedFilesToYes()
    {
        DeleteUploadedFilesButton.Text = "Deletion of uploaded files [Yes]";
    }

    public void SetDeletionOfUploadedFilesToNo()
    {
        DeleteUploadedFilesButton.Text = "Deletion of uploaded files [No]";
    }

    public void ShowMessageBox(string message)
    {
        MessageBox.Show(message);
    }
}