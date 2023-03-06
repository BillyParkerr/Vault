namespace Application.Views;

public partial class ExportEncryptedFileView : Form, IExportEncryptedFileView
{
    public event EventHandler ConfirmEvent;

    public ExportEncryptedFileView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public string GivenPassword
    {
        get { return PasswordTextBox.Text;  }
    }

    public void ShowBlankPasswordError()
    {
        MessageBox.Show("No password was given! Please enter a password.");
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