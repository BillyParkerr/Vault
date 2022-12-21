using Application.Helpers;

namespace Application.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (EncryptionHelper.VerifyPassword(textBox1.Text))
                {
                    DialogResult = DialogResult.OK;
                    UserInformation.LoginInfomation.Password = textBox1.Text;
                }
                else
                {
                    MessageBox.Show("Incorrect Password Entered! Please Try Again.");
                }
            }
        }
    }
}