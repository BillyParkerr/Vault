namespace Application.Views;

public partial class WindowsHelloRegisterView : Form, IWindowsHelloRegisterView
{
    public event EventHandler ConfirmEvent;

    public WindowsHelloRegisterView()
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
        ConfirmButton.Click += delegate { ConfirmEvent?.Invoke(this, EventArgs.Empty); };
    }
}