using System.Text;
using Application.Enums;

namespace Application.Managers;

public class LoginManager : ILoginManager
{
    private IDatabaseManager databaseManager;
    private IEncryptionManager encryptionManager;
    private IFileManager fileManager;
    private AppSettings appSettings;

    public LoginManager(IDatabaseManager databaseManager, IEncryptionManager encryptionManager, IFileManager fileManager, AppSettings appSettings)
    {
        this.databaseManager = databaseManager;
        this.encryptionManager = encryptionManager;
        this.fileManager = fileManager;
        this.appSettings = appSettings;
    }

    public bool VerifyPassword(string password)
    {
        // Get EncryptionKey from database
        var encryptionKey = databaseManager.GetEncryptionKey().Key;
        try
        {
            var decryptedKey = encryptionManager.DecryptString(encryptionKey, password);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void SetPassword(string password)
    {
        var baseString = GenerateRandomStringForEncryptionKey();
        var encryptedBaseString = encryptionManager.EncryptString(baseString, password);
        databaseManager.SetEncryptionKey(encryptedBaseString);
        databaseManager.SaveChanges();
        encryptionManager.SetEncryptionPassword(password);
        if (appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
        {
            fileManager.ProtectAndSavePassword(password);
        }
    }

    private static string GenerateRandomStringForEncryptionKey()
    {
        Random random = new Random();
        int length = random.Next(20, 26);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder stringBuilder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}
