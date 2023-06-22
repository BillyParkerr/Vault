using Application.Views.Interfaces;

namespace Application.Views.Forms;

public partial class LoginView : Form, ILoginView
{
    public event EventHandler LoginEvent;

    public LoginView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public string GivenPassword
    {
        get { return textBox1.Text; }
    }

    private void AssociateAndRaiseViewEvents()
    {
        LoginButton.Click += delegate { LoginEvent?.Invoke(this, EventArgs.Empty); };
    }

    public void ShowBlankPasswordGivenError()
    {
        MessageBox.Show("No password was given! Please enter a password.");
    }

    public void ShowIncorrectPasswordError()
    {
        MessageBox.Show("The given password was not correct! Please try again.");
    }
}