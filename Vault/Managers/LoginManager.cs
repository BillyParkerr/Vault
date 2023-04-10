using Application.Enums;
using System.Text;

namespace Application.Managers;

public class LoginManager : ILoginManager
{
    private readonly IDatabaseManager _databaseManager;
    private readonly IEncryptionManager _encryptionManager;
    private readonly IFileManager _fileManager;
    private readonly AppSettings _appSettings;

    public LoginManager(IDatabaseManager databaseManager, IEncryptionManager encryptionManager, IFileManager fileManager, AppSettings appSettings)
    {
        _databaseManager = databaseManager;
        _encryptionManager = encryptionManager;
        _fileManager = fileManager;
        _appSettings = appSettings;
    }

    /// <summary>
    /// Verifies if the given password is correct by attempting to decrypt the stored encryption key.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <returns>True if the password is correct, otherwise false.</returns>
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

    /// <summary>
    /// Sets the password for the application by encrypting a randomly generated string, setting the encryption key in the database,
    /// and updating the encryption password in the encryption manager.
    /// </summary>
    /// <param name="password">The password to set.</param>
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

    /// <summary>
    /// Changes the application password by decrypting the current encryption key with the old password, re-encrypting the key with the new password,
    /// updating the encryption key in the database, and setting the new password.
    /// </summary>
    /// <param name="newPassword">The new password to set.</param>
    /// <param name="oldPassword">The old password to validate.</param>
    /// <returns>True if the password change is successful, otherwise false.</returns>
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
