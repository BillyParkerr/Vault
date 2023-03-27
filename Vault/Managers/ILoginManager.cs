namespace Application.Managers;

public interface ILoginManager
{
    void SetPassword(string password);
    bool VerifyPassword(string password);
    bool ChangePassword(string newPassword, string oldPassword);
}