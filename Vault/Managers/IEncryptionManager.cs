namespace Application.Managers;

public interface IEncryptionManager
{
    string EncryptFile(string sourceFilePath, string? password = null);
    string DecryptFile(string sourceFilePath, string? password = null, string? destinationPath = null);
    string DecryptFileName(string encryptedFileName, string? password = null);
    bool VerifyPassword(string password);
}