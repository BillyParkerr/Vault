namespace Application.Managers;

public interface IEncryptionManager
{
    string EncryptFile(string sourceFilePath, string password = null);
    string DecryptFile(string sourceFilePath, string destinationPath = null, string password = null);
    string EncryptString(string plaintext, string password = null);
    string DecryptString(string encryptedFileName, string password = null);
    void SetEncryptionPassword(string password);
}