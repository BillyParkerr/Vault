using Application.Views.Interfaces;

namespace Application.Views.Forms;

public partial class RegisterView : Form, IRegisterView
{
    public event EventHandler RegisterEvent;

    public RegisterView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public string GivenPassword
    {
        get { return PasswordTextBox.Text;  }
    }

    public string GivenSecondPassword
    {
        get { return ReEnterPasswordTextBox.Text; }
    }

    public void ShowBlankPasswordError()
    {
        MessageBox.Show("No password was given! Please enter a password.");
    }

    public void ShowPasswordMismatchError()
    {
        MessageBox.Show("The given passwords do not match! Please try again.");
    }

    public void ShowPasswordTooShortError()
    {
        MessageBox.Show("The given password is too short! Please ensure password is at least six characters long.");
    }

    private void AssociateAndRaiseViewEvents()
    {
        RegisterButton.Click += delegate { RegisterEvent?.Invoke(this, EventArgs.Empty); };
    }
}