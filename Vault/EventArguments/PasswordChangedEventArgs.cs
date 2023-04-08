namespace Application.EventArguments;

public class PasswordChangedEventArgs : System.EventArgs
{
    public string EnteredOldPassword { get; set; }
    public string EnteredNewPassword { get; set; }

    public PasswordChangedEventArgs(string oldPassword, string newPassword)
    {
        EnteredOldPassword = oldPassword;
        EnteredNewPassword = newPassword;
    }
}