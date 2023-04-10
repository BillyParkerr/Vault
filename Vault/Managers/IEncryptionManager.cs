namespace Application.Managers;

public interface IEncryptionManager
{

    /// <summary>
    /// Sets the encryption password for the EncryptionManager instance.
    /// </summary>
    /// <param name="password"></param>
    void SetEncryptionPassword(string password);

    /// <summary>Encrypts a file located at the given sourceFilePath using the specified password (if provided) or the instance password.</summary>
    /// <param name="sourceFilePath">The full path and name of the file to be encrypted.</param>
    /// <param name="password"></param>
    /// <returns>The path of the encrypted file.</returns>
    string EncryptFile(string sourceFilePath, string password = null);

    /// <summary>
    /// Encrypts a given plaintext string using the specified password (if provided) or the instance password.
    /// </summary>
    /// <param name="plaintext">The text which is to be encrypted</param>
    /// <param name="password"></param>
    /// <returns>Encrypted string</returns>
    string EncryptString(string plaintext, string password = null);

    /// <summary>
    /// Decrypts a file located at the given sourceFilePath using the specified password (if provided) or the instance password.
    /// If the destinationPath is provided, the decrypted file will be saved there, otherwise it will be saved in the default decrypted files location.
    /// </summary>
    /// <remarks>NB: "Padding is invalid and cannot be removed." is the Universal CryptoServices error.  Make sure the password, salt and iterations are correct before getting nervous.</remarks>
    /// <returns>The path of the DecryptedFile</returns>
    /// <exception cref="ApplicationException"></exception>
    string DecryptFile(string sourceFilePath, string destinationPath = null, string password = null);

    /// <summary>
    /// Decrypts an encrypted file name using the specified password (if provided) or the instance password.
    /// </summary>
    /// <param name="encryptedFileName">A combiniation of the ciphertext, nonce and salt in the form of a file name</param>
    /// <param name="password"></param>
    /// <returns>The decrypted string</returns>
    string DecryptString(string encryptedFileName, string password = null);
}