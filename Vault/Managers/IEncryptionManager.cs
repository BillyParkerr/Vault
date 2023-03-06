namespace Application.Managers;

public interface IEncryptionManager
{
    string EncryptFile(string sourceFilePath, string password = null);
    string DecryptFile(string sourceFilePath, string destinationPath = null, string password = null);
    string DecryptString(string encryptedFileName, string password = null);
    bool VerifyPassword(string password);
    void SetPassword(string password);
}