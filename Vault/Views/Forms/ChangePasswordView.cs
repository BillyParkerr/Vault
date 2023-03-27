using Application.Views.Interfaces;

namespace Application.Views.Forms;

public partial class ChangePasswordView : Form, IChangePasswordView
{
    public event EventHandler ConfirmPasswordEvent;

    public ChangePasswordView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public string GivenOldPassword
    {
        get { return OldPasswordTextBox.Text; }
    }

    public string GivenNewPassword
    {
        get { return PasswordTextBox.Text;  }
    }

    public string GivenSecondNewPassword
    {
        get { return ReEnterPasswordTextBox.Text; }
    }

    public void ShowIncorrectOldPasswordError()
    {
        MessageBox.Show("The old password given was incorrect! Please retry.");
    }

    public void ShowBlankNewPasswordError()
    {
        MessageBox.Show("No new password was given! Please enter a password.");
    }

    public void ShowBlankOldPasswordError()
    {
        MessageBox.Show("No old password was given! Please enter a password.");
    }

    public void ShowPasswordMismatchError()
    {
        MessageBox.Show("The given new passwords do not match! Please try again.");
    }

    public void ShowNewPasswordTooShortError()
    {
        MessageBox.Show("The given new password is too short! Please ensure password is at least six characters long.");
    }

    private void AssociateAndRaiseViewEvents()
    {
        ConfirmButton.Click += delegate { ConfirmPasswordEvent?.Invoke(this, EventArgs.Empty); };
    }
}