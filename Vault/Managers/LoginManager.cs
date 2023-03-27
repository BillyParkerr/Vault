using System.Reflection.Metadata.Ecma335;
using System.Text;
using Application.Enums;

namespace Application.Managers;

public class LoginManager : ILoginManager
{
    private readonly IDatabaseManager databaseManager;
    private readonly IEncryptionManager encryptionManager;
    private readonly IFileManager fileManager;
    private readonly AppSettings appSettings;

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

    public bool ChangePassword(string newPassword, string oldPassword)
    {
        var currentEncryptionKey = databaseManager.GetEncryptionKey();
        string decryptedKey;
        try
        {
            decryptedKey = encryptionManager.DecryptString(currentEncryptionKey.Key, oldPassword);
        }
        catch
        {
            return false;
        }

        if (decryptedKey != null)
        {
            var newEncryptionKey = encryptionManager.EncryptString(decryptedKey, newPassword);
            databaseManager.ChangeEncryptionKey(newEncryptionKey);
            databaseManager.SaveChanges();
            encryptionManager.SetEncryptionPassword(newPassword);
            if (appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
            {
                fileManager.ProtectAndSavePassword(newPassword);
            }
        }
        else
        {
            return false;
        }


        return true;
    }

    private static string GenerateRandomStringForEncryptionKey()
    {
        Random random = new();
        int length = random.Next(20, 26);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder stringBuilder = new(length);
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}
