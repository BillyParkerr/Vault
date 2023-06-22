namespace Application.EventArguments;

public class PasswordVerificationCompleteEventArgs : EventArgs
{
    public bool PasswordVerified { get; set; }
    public string GivenPassword { get; set; }

    public PasswordVerificationCompleteEventArgs(bool passwordVerified, string password)
    {
        PasswordVerified = passwordVerified;
        GivenPassword = password;
    }
}