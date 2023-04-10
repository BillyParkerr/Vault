using NPOI.Util;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Array = System.Array;

namespace Application.Managers;

// Below I will give a breif explanation as to how the encryption / decrpytion process works
//
// An important thing to keep in mind is that the file name and file contents follow different encryption processes. However both use AES encryption.
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
            // TODO add logging maybe?
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

    private static byte[] GenerateInitilisationVector()
    {
        // Generate a 96-bit initilisation vector using a CSPRNG.
        SecureRandom rand = new();
        byte[] initilisationVector = new byte[AlgorithmnNonceSize];
        rand.NextBytes(initilisationVector);

        return initilisationVector;
    }

    /// <summary>
    /// Reads the starting contents of an encrypted file in order to retreive the IV and Salt.
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <returns>A Tuple containing IV and Salt in that order</returns>
    private static (byte[], byte[]) GetInitisationVectorAndSaltFromEncryptedFile(string fileLocation)
    {
        var buffer = new byte[32];
        try
        {
            using var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
            var bytesRead = fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            if (bytesRead != buffer.Length)
            {
                // TODO Add logging here
            }

            var iv = buffer.Take(AlgorithmnNonceSize).ToArray();
            var salt = buffer.Skip(AlgorithmnNonceSize).Take(Pbkdf2SaltSize).ToArray();

            return (iv, salt);
        }
        catch (UnauthorizedAccessException ex)
        {
            Debug.Print(ex.Message);
        }

        throw new("Unable to get IV and Salt from EncryptedFile.");
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

        var initilisationVector = GenerateInitilisationVector();
        var salt = GenerateSalt();
        var key = GenerateKeyFromSaltAndPassword(salt, password);

        var tempStream = new ByteArrayOutputStream();
        tempStream.Write(initilisationVector);
        tempStream.Write(salt);
        byte[] iVAndSalt = tempStream.ToByteArray();

        // Create an Aes object
        // with the specified key and IV.
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = initilisationVector;

        // Create an encryptor to perform the stream transform.
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using var destination = new FileStream(@destinationFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        destination.BeginWrite(iVAndSalt, 0, iVAndSalt.Length, null, null);
        using var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write);
        using var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        source.CopyTo(cryptoStream);

        return destinationFilePath;
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

        // Encrypt and prepend salt.
        byte[] ciphertextAndNonce = Encrypt(Encoding.UTF8.GetBytes(plaintext), key);
        byte[] ciphertextAndNonceAndSalt = new byte[salt.Length + ciphertextAndNonce.Length];
        Array.Copy(salt, 0, ciphertextAndNonceAndSalt, 0, salt.Length);
        Array.Copy(ciphertextAndNonce, 0, ciphertextAndNonceAndSalt, salt.Length, ciphertextAndNonce.Length);

        // Return as base64 string.
        return Convert.ToBase64String(ciphertextAndNonceAndSalt).Replace(@"/", "_"); // The .Replace is important here as / cannot be in file names in windows.
                                                                                     // There are several other illegal characters in windows but this is the only
                                                                                     // One used as part of AES encryption. This process is inverted for Decrypting.
    }

    private static byte[] Encrypt(byte[] plaintext, byte[] key)
    {
        // Generate a 96-bit nonce using a CSPRNG.
        SecureRandom rand = new();
        byte[] nonce = new byte[AlgorithmnNonceSize];
        rand.NextBytes(nonce);

        // Create the cipher instance and initialize.
        GcmBlockCipher cipher = new(new AesEngine());
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("AES", key);
        ParametersWithIV cipherParameters = new(keyParam, nonce);
        cipher.Init(true, cipherParameters);

        // Encrypt and prepend nonce.
        byte[] ciphertext = new byte[cipher.GetOutputSize(plaintext.Length)];
        int length = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
        cipher.DoFinal(ciphertext, length);

        byte[] ciphertextAndNonce = new byte[nonce.Length + ciphertext.Length];
        Array.Copy(nonce, 0, ciphertextAndNonce, 0, nonce.Length);
        Array.Copy(ciphertext, 0, ciphertextAndNonce, nonce.Length, ciphertext.Length);

        return ciphertextAndNonce;
    }

    #endregion

    #region Decryption

    /// <summary>
    /// Decrypts a file located at the given sourceFilePath using the specified password (if provided) or the instance password.
    /// If the destinationPath is provided, the decrypted file will be saved there, otherwise it will be saved in the default decrypted files location.
    /// </summary>
    /// <remarks>NB: "Padding is invalid and cannot be removed." is the Universal CryptoServices error.  Make sure the password, salt and iterations are correct before getting nervous.</remarks>
    /// <returns>The path of the DecryptedFile</returns>
    /// <exception cref="ApplicationException"></exception>
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

        // Create an Aes object
        // with the specified key and IV.
        using Aes aesAlg = Aes.Create();
        var ivAndSalt = GetInitisationVectorAndSaltFromEncryptedFile(sourceFilePath);
        var iv = ivAndSalt.Item1;
        var salt = ivAndSalt.Item2;
        var key = GenerateKeyFromSaltAndPassword(salt, password);
        aesAlg.Key = key;
        aesAlg.IV = iv;

        ICryptoTransform transform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using FileStream destination = new(destinationFileLocation, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using CryptoStream cryptoStream = new(destination, transform, CryptoStreamMode.Write);
        try
        {
            using FileStream source = new(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            source.Position = AlgorithmnNonceSize + Pbkdf2SaltSize; // Skip the IV and Salt porition of the stream
            source.CopyTo(cryptoStream);
        }
        catch (CryptographicException exception)
        {
            if (exception.Message == "Padding is invalid and cannot be removed.")
            {
                throw new ApplicationException("Universal Microsoft Cryptographic Exception (Not to be believed!)", exception);
            }
            else
            {
                throw;
            }
        }

        return destinationFileLocation;
    }

    /// <summary>
    /// Decrypts an encrypted file name using the specified password (if provided) or the instance password.
    /// </summary>
    /// <param name="encryptedFileName">A combiniation of the ciphertext, nonce and salt in the form of a file name</param>
    /// <param name="password"></param>
    /// <returns>The decrypted string</returns>
    public string DecryptString(string encryptedFileName, string password = null)
    {
        if (password == null)
        {
            IsPasswordSet();
            password = DecryptedEncryptionKey;
        }

        // Decode the base64.
        byte[] ciphertextAndNonceAndSalt = Convert.FromBase64String(encryptedFileName.Replace("_", @"/")); // The .Replace is important here as / cannot be in file names in windows.
                                                                                                                            // There are several other illegal characters in windows only this is relevant for this flow.
        byte[] salt = new byte[Pbkdf2SaltSize];                                                                            // Retrieve the salt and ciphertextAndNonce. This process is inverted for Encrypting.
        byte[] ciphertextAndNonce = new byte[ciphertextAndNonceAndSalt.Length - Pbkdf2SaltSize];
        Array.Copy(ciphertextAndNonceAndSalt, 0, salt, 0, salt.Length);
        Array.Copy(ciphertextAndNonceAndSalt, salt.Length, ciphertextAndNonce, 0, ciphertextAndNonce.Length);

        var key = GenerateKeyFromSaltAndPassword(salt, password);

        // Decrypt and return result.
        return Encoding.UTF8.GetString(Decrypt(ciphertextAndNonce, key));
    }

    private static byte[] Decrypt(byte[] ciphertextAndNonce, byte[] key)
    {
        // Retrieve the nonce and ciphertext.
        byte[] nonce = new byte[AlgorithmnNonceSize];
        byte[] ciphertext = new byte[ciphertextAndNonce.Length - AlgorithmnNonceSize];
        Array.Copy(ciphertextAndNonce, 0, nonce, 0, nonce.Length);
        Array.Copy(ciphertextAndNonce, nonce.Length, ciphertext, 0, ciphertext.Length);

        // Create the cipher instance and initialize.
        GcmBlockCipher cipher = new(new AesEngine());
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("AES", key);
        ParametersWithIV cipherParameters = new(keyParam, nonce);
        cipher.Init(false, cipherParameters);

        // Decrypt and return result.
        byte[] plaintext = new byte[cipher.GetOutputSize(ciphertext.Length)];
        int length = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plaintext, 0);
        cipher.DoFinal(plaintext, length);

        return plaintext;
    }

    #endregion
}