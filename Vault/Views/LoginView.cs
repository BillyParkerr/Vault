namespace Application.Views;

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
        get { return textBox1.Text;  }
    }

    private void AssociateAndRaiseViewEvents()
    {
        LoginButton.Click += delegate { LoginEvent?.Invoke(this, EventArgs.Empty); };
    }
}