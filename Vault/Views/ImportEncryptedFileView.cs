namespace Application.Views;

public partial class ImportEncryptedFileView : Form, IImportEncryptedFileView
{
    public event EventHandler ConfirmEvent;

    public ImportEncryptedFileView()
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

    private void AssociateAndRaiseViewEvents()
    {
        ConfirmButton.Click += delegate { ConfirmEvent?.Invoke(this, EventArgs.Empty); };
    }
}