namespace Application.Managers;

public class WindowsHelloEncryptionManager : IEncryptionManager
{
    private IDatabaseManager databaseManager;

    public WindowsHelloEncryptionManager(IDatabaseManager databaseManager)
    {
        this.databaseManager = databaseManager;
    }

    public string DecryptFile(string sourceFilePath, string destinationPath = null, string password = null)
    {
        throw new NotImplementedException();
    }

    public string DecryptString(string encryptedFileName, string password = null)
    {
        throw new NotImplementedException();
    }

    public string EncryptFile(string sourceFilePath, string password = null)
    {
        throw new NotImplementedException();
    }

    public void SetPassword(string password)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPassword(string password)
    {
        throw new NotImplementedException();
    }
}
