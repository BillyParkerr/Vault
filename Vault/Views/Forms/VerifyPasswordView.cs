using Application.Views.Interfaces;
using SQLitePCL;

namespace Application.Views.Forms;

public partial class VerifyPasswordView : Form, IVerifyPasswordView
{
    public event EventHandler VerifyEvent;
    public event EventHandler UserClosedFormEvent;
    private bool PasswordVerified = false;

    public VerifyPasswordView()
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
        VerifyButton.Click += delegate
        {
            PasswordVerified = true;
            VerifyEvent?.Invoke(this, EventArgs.Empty);
        };
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!PasswordVerified)
        {
            DialogResult result = MessageBox.Show("Windows Hello Authentication will not be used. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                UserClosedFormEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        base.OnFormClosing(e);
    }

    public void ShowBlankPasswordGivenError()
    {
        MessageBox.Show("No password was given! Please enter a password.");
        PasswordVerified = false;
    }

    public void ShowIncorrectPasswordError()
    {
        MessageBox.Show("The given password was not correct! Please try again.");
        PasswordVerified = false;
    }
}