namespace Application.Managers;

public class PasswordLoginManager
{
    private IDatabaseManager databaseManager;
    private IEncryptionManager encryptionManager;

    public PasswordLoginManager(IDatabaseManager databaseManager, IEncryptionManager encryptionManager)
    {
        this.databaseManager = databaseManager;
        this.encryptionManager = encryptionManager;
    }

    public bool VerifyPassword(string password)
    {
        // Get EncryptionKey from database
        var encryptionKey = databaseManager.GetEncryptionKey().Key;
        try
        {
            var decryptedKey = encryptionManager.DecryptString(encryptionKey, password);
            decryptedEncryptionKey = decryptedKey;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
