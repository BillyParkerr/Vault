//using Application.Managers;

//namespace Application.Views;

//public partial class Login : Form
//{
//    public Login()
//    {
//        InitializeComponent();
//    }

//    private void Form1_Load(object sender, EventArgs e)
//    {

//    }

//    private void button1_Click(object sender, EventArgs e)
//    {
//        if (!string.IsNullOrWhiteSpace(textBox1.Text))
//        {
//            if (EncryptionManager.VerifyPassword(textBox1.Text))
//            {
//                DialogResult = DialogResult.OK;
//                LoginInfomation.Password = textBox1.Text;
//            }
//            else
//            {
//                MessageBox.Show("Incorrect Password Entered! Please Try Again.");
//            }
//        }
//    }
//}