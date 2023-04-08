using System.Reflection.Metadata.Ecma335;
using System.Text;
using Application.Enums;

namespace Application.Managers;

public class LoginManager : ILoginManager
{
    private readonly IDatabaseManager _databaseManager;
    private readonly IEncryptionManager _encryptionManager;
    private readonly IFileManager _fileManager;
    private readonly AppSettings _appSettings;

    public LoginManager(IDatabaseManager databaseManager, IEncryptionManager encryptionManager, IFileManager fileManager, AppSettings appSettings)
    {
        this._databaseManager = databaseManager;
        this._encryptionManager = encryptionManager;
        this._fileManager = fileManager;
        this._appSettings = appSettings;
    }

    public bool VerifyPassword(string password)
    {
        // Get EncryptionKey from database
        var encryptionKey = _databaseManager.GetEncryptionKey().Key;
        try
        {
            var decryptedKey = _encryptionManager.DecryptString(encryptionKey, password);
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
        var encryptedBaseString = _encryptionManager.EncryptString(baseString, password);
        _databaseManager.SetEncryptionKey(encryptedBaseString);
        _databaseManager.SaveChanges();
        _encryptionManager.SetEncryptionPassword(password);
        if (_appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
        {
            _fileManager.ProtectAndSavePassword(password);
        }
    }

    public bool ChangePassword(string newPassword, string oldPassword)
    {
        var currentEncryptionKey = _databaseManager.GetEncryptionKey();
        string decryptedKey;
        try
        {
            decryptedKey = _encryptionManager.DecryptString(currentEncryptionKey.Key, oldPassword);
        }
        catch
        {
            return false;
        }

        if (decryptedKey != null)
        {
            var newEncryptionKey = _encryptionManager.EncryptString(decryptedKey, newPassword);
            _databaseManager.ChangeEncryptionKey(newEncryptionKey);
            _databaseManager.SaveChanges();
            _encryptionManager.SetEncryptionPassword(newPassword);
            if (_appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
            {
                _fileManager.ProtectAndSavePassword(newPassword);
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
