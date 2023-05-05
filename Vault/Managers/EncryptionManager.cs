using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;
using Array = System.Array;

namespace Application.Managers;

// Below I will give a breif explanation as to how the encryption / decrpytion process works
//
// An important thing to keep in mind is that the file name and file contents follow different encryption processes. However both use AES-GCM.
//
// The Encryption process for a file goes as follows:
// 1 - Encrypting the file name. (The file name is encrypted seperatly so it can be decrypted without having to decrypt the entire file)
// 1.1 - Generate a random salt.
// 1.2 - Generate a key based upon the password and salt.
// 1.3 - We then generate a random nonce
// 1.4 - The file name is then encrypted using the nonce and key.
// 1.5 - We then write the salt followed by the nonce to the start of the file name. (This done so that we can use it for decryption)
// 1.6 - At this point we have a byte array so we convert it to a string and add.aes on the end.

// 2 - Encrypting the file contents
// 2.1 - Generate a random initilisation vector (IV). This can also be reffered to as a nonce.
// 2.2 - Generate a random salt.
// 2.3 - Generate a key from the password and salt to use for. (This ensures that each key is different even when using the same password)
// 2.4 - We first write the IV and Salt to the encrypted file using the file name we generated previously.
// 2.4 - We then encrypt the contents and write it to the file as part of a Crypto and FileStream.

// The Decryption process for a file goes as follow:
// 1 - Decrypt the file name
// 1.1 - Extract the salt from the start of the file
// 1.2 - Recreate the key used for the encryption using the salt and the password.
// 1.3 - Seperate the salt and cipher text (encyrpted file contents) into seperate variables.
// 1.4 - Decrypt the cipher text using the key and nonce. This will give us our decrypted file name.
//
// 2 - Decrypt the file contents
// 2.1 - Get the IV and Salt from the start of the encrypted file contents.
// 2.2 - Recreate the key used for the encryption using the retreived salt and the password.
// 2.3 - Decrypt the contents of the file using the key and IV. Ensuring that we skip the length of the IV and Salt at the start.

/// <summary>
/// This class is responsible for all encryption and decryption that takes place within the application.
/// </summary>
public class EncryptionManager : IEncryptionManager
{
    // These allow for the adjustment of the encryption algorithm variables. In this class AES encryption is used.
    // Please note that the adjustment of these values will cause previously encrypted files to not be able to be decrypted!
    private const int AlgorithmnNonceSize = 16;
    private const int AlgorithmKeySize = 32;
    private const int Pbkdf2SaltSize = 16;
    private const int Pbkdf2Iterations = 32767;
    private protected string DecryptedEncryptionKey;
    private readonly IDatabaseManager _databaseManager;

    public EncryptionManager(IDatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Sets the encryption password for the EncryptionManager instance.
    /// </summary>
    /// <param name="password"></param>
    public void SetEncryptionPassword(string password)
    {
        var encryptionKey = _databaseManager.GetEncryptionKey();
        try
        {
            var decryptedKey = DecryptString(encryptionKey.Key, password);
            DecryptedEncryptionKey = decryptedKey;
        }
        catch
        {
            return;
        }
    }

    private void IsPasswordSet()
    {
        if (DecryptedEncryptionKey == null)
        {
            throw new ApplicationException("The password must be set before any encrytion can take place!");
        }
    }

    private static byte[] GenerateSalt()
    {
        // Generate a 128-bit salt using a CSPRNG.
        SecureRandom rand = new();
        byte[] salt = new byte[Pbkdf2SaltSize];
        rand.NextBytes(salt);

        return salt;
    }

    private byte[] GenerateKeyFromSaltAndPassword(byte[] salt, string password = null)
    {
        password ??= DecryptedEncryptionKey;

        // Create an instance of PBKDF2 and derive a key.
        Pkcs5S2ParametersGenerator pbkdf2 = new(new Sha256Digest());
        pbkdf2.Init(Encoding.UTF8.GetBytes(password), salt, Pbkdf2Iterations);
        byte[] key = ((KeyParameter)pbkdf2.GenerateDerivedMacParameters(AlgorithmKeySize * 8)).GetKey();

        return key;
    }

    private static GcmBlockCipher InitializeCipher(bool forEncryption, byte[] key, byte[] nonce)
    {
        GcmBlockCipher cipher = new(new AesEngine());
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("AES", key);
        ParametersWithIV cipherParameters = new(keyParam, nonce);
        cipher.Init(forEncryption, cipherParameters);

        return cipher;
    }

    #region Encryption

    /// <summary>Encrypts a file located at the given sourceFilePath using the specified password (if provided) or the instance password.</summary>
    /// <param name="sourceFilePath">The full path and name of the file to be encrypted.</param>
    /// <param name="password"></param>
    /// <returns>The path of the encrypted file.</returns>
    public string EncryptFile(string sourceFilePath, string password = null)
    {
        if (password == null)
        {
            IsPasswordSet();
            password = DecryptedEncryptionKey;
        }

        var destinationFileName = EncryptString(Path.GetFileName(sourceFilePath), password);
        var destinationFilePath = Path.Combine(DirectoryPaths.EncryptedFilesCommonDirectory, destinationFileName + ".aes");

        var nonce = GenerateNonce();
        var salt = GenerateSalt();
        var key = GenerateKeyFromSaltAndPassword(salt, password);

        using FileStream sourceStream = File.OpenRead(sourceFilePath);
        using FileStream destinationStream = File.Create(destinationFilePath);

        WriteSaltAndNonce(destinationStream, salt, nonce);

        GcmBlockCipher cipher = InitializeCipher(true, key, nonce);

        EncryptAndWriteData(cipher, sourceStream, destinationStream);

        return destinationFilePath;
    }

    private static void WriteSaltAndNonce(FileStream destinationStream, byte[] salt, byte[] nonce)
    {
        destinationStream.Write(salt, 0, salt.Length);
        destinationStream.Write(nonce, 0, nonce.Length);
    }

    private static void EncryptAndWriteData(GcmBlockCipher cipher, FileStream sourceStream, FileStream destinationStream)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            byte[] ciphertext = new byte[cipher.GetOutputSize(bytesRead)];
            int length = cipher.ProcessBytes(buffer, 0, bytesRead, ciphertext, 0);

            destinationStream.Write(ciphertext, 0, length);
        }

        byte[] finalOutput = new byte[cipher.GetOutputSize(0)];
        int finalLength = cipher.DoFinal(finalOutput, 0);
        destinationStream.Write(finalOutput, 0, finalLength);
    }

    /// <summary>
    /// Encrypts a given plaintext string using the specified password (if provided) or the instance password.
    /// </summary>
    /// <param name="plaintext">The text which is to be encrypted</param>
    /// <param name="password"></param>
    /// <returns>Encrypted string</returns>
    public string EncryptString(string plaintext, string password = null)
    {
        password ??= DecryptedEncryptionKey;

        var salt = GenerateSalt();
        var key = GenerateKeyFromSaltAndPassword(salt, password);
        var nonce = GenerateNonce();

        // Encrypt and prepend salt.
        byte[] ciphertext = EncryptData(Encoding.UTF8.GetBytes(plaintext), key, nonce);
        byte[] ciphertextNonceAndSalt = CombineSaltNonceAndCipherText(salt, nonce, ciphertext);

        // Return as base64 string.
        return Convert.ToBase64String(ciphertextNonceAndSalt).Replace(@"/", "_"); // The .Replace is important here as / cannot be in file names in windows.
                                                                                     // There are several other illegal characters in windows but this is the only
                                                                                     // one used as part of AES encryption. This process is inverted for Decrypting.
    }

    private static byte[] EncryptData(byte[] data, byte[] key, byte[] nonce)
    {
        // Create the cipher instance and initialize.
        GcmBlockCipher cipher = InitializeCipher(true, key, nonce);

        // Encrypt data.
        byte[] ciphertext = new byte[cipher.GetOutputSize(data.Length)];
        int length = cipher.ProcessBytes(data, 0, data.Length, ciphertext, 0);
        cipher.DoFinal(ciphertext, length);

        return ciphertext;
    }

    private static byte[] GenerateNonce()
    {
        // Generate a 96-bit nonce using a CSPRNG.
        SecureRandom rand = new();
        byte[] nonce = new byte[AlgorithmnNonceSize];
        rand.NextBytes(nonce);

        return nonce;
    }

    private static byte[] CombineSaltNonceAndCipherText(byte[] salt, byte[] nonce, byte[] cipherText)
    {
        byte[] saltNonceAndCiphertext = new byte[nonce.Length + cipherText.Length + salt.Length];
        Array.Copy(salt, 0, saltNonceAndCiphertext, 0, salt.Length);
        Array.Copy(nonce, 0, saltNonceAndCiphertext, salt.Length, nonce.Length);
        Array.Copy(cipherText, 0, saltNonceAndCiphertext, salt.Length + nonce.Length, cipherText.Length);

        return saltNonceAndCiphertext;
    }

    #endregion

    #region Decryption

    public string DecryptFile(string sourceFilePath, string destinationPath = null, string password = null)
    {
        if (password == null)
        {
            IsPasswordSet();
            password = DecryptedEncryptionKey;
        }
        destinationPath ??= DirectoryPaths.DecryptedFilesCommonDirectory;

        var destinationFileName = DecryptString(Path.GetFileNameWithoutExtension(sourceFilePath), password);
        var destinationFileLocation = Path.Combine(destinationPath, destinationFileName);

        using FileStream sourceStream = File.OpenRead(sourceFilePath);
        using FileStream destinationStream = File.Create(destinationFileLocation);

        (byte[] salt, byte[] nonce) = ReadSaltAndNonceFromStream(sourceStream);

        byte[] key = GenerateKeyFromSaltAndPassword(salt, password);

        GcmBlockCipher cipher = InitializeCipher(false, key, nonce);

        DecryptAndWriteData(cipher, sourceStream, destinationStream);

        return destinationFileLocation;
    }

    /// <summary>
    /// Decrypts an encrypted string using the specified password (if provided) or the instance password.
    /// </summary>
    /// <param name="encryptedString">A combiniation of the ciphertext, nonce and salt.></param>
    /// <param name="password"></param>
    /// <returns>The decrypted string</returns>
    public string DecryptString(string encryptedString, string password = null)
    {
        if (password == null)
        {
            IsPasswordSet();
            password = DecryptedEncryptionKey;
        }

        // Decode the base64.
        byte[] saltNonceAndCipherText = Convert.FromBase64String(encryptedString.Replace("_", "/"));  // The .Replace is important here as / cannot be in file names in windows.
                                                                                                                          // There are several other illegal characters in windows only this is relevant for this flow.
                                                                                                                          // This process is inverted for Encrypting.
        // Split the salt, nonce, and ciphertext.
        (byte[] salt, byte[] nonce, byte[] ciphertext) = SplitSaltNonceAndCipherText(saltNonceAndCipherText);

        // Generate the key.
        byte[] key = GenerateKeyFromSaltAndPassword(salt, password);

        // Decrypt and return the result.
        return Encoding.UTF8.GetString(DecryptByteData(ciphertext, key, nonce));
    }

    private static void DecryptAndWriteData(GcmBlockCipher cipher, FileStream sourceStream, FileStream destinationStream)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            byte[] plaintext = new byte[cipher.GetOutputSize(bytesRead)];
            int length = cipher.ProcessBytes(buffer, 0, bytesRead, plaintext, 0);

            destinationStream.Write(plaintext, 0, length);
        }

        byte[] finalOutput = new byte[cipher.GetOutputSize(0)];
        int finalLength = cipher.DoFinal(finalOutput, 0);
        destinationStream.Write(finalOutput, 0, finalLength);
    }

    private static byte[] DecryptByteData(byte[] ciphertext, byte[] key, byte[] nonce)
    {
        // Create the cipher instance and initialize.
        GcmBlockCipher cipher = InitializeCipher(false, key, nonce);

        // Decrypt and return result.
        byte[] plaintext = new byte[cipher.GetOutputSize(ciphertext.Length)];
        int length = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plaintext, 0);
        cipher.DoFinal(plaintext, length);

        return plaintext;
    }

    private static (byte[] salt, byte[] nonce, byte[] cipherText) SplitSaltNonceAndCipherText(byte[] saltNonceAndCipherText)
    {
        byte[] salt = new byte[Pbkdf2SaltSize];
        byte[] nonce = new byte[AlgorithmnNonceSize];
        byte[] cipherText = new byte[saltNonceAndCipherText.Length - salt.Length - nonce.Length];

        Array.Copy(saltNonceAndCipherText, 0, salt, 0, salt.Length);
        Array.Copy(saltNonceAndCipherText, salt.Length, nonce, 0, nonce.Length);
        Array.Copy(saltNonceAndCipherText, salt.Length + nonce.Length, cipherText, 0, cipherText.Length);

        return (salt, nonce, cipherText);
    }

    private static (byte[] salt, byte[] nonce) ReadSaltAndNonceFromStream(FileStream sourceStream)
    {
        byte[] salt = new byte[Pbkdf2SaltSize];
        byte[] nonce = new byte[AlgorithmnNonceSize];
        sourceStream.Read(salt, 0, salt.Length);
        sourceStream.Read(nonce, 0, nonce.Length);

        return (salt, nonce);
    }


    #endregion
}