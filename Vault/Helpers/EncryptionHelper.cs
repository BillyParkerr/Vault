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

namespace Application.Helpers;

public static class EncryptionHelper
{
    // These allow for the adjustment of the encryption algorithm variables. In this class AES encryption is used.
    // Please note that the adjustment of these values will cause previously encrypted files to not be able to be decrypted!
    private const int AlgorithmnNonceSize = 16;
    private const int AlgorithmKeySize = 32;
    private const int PBKDF2_SaltSize = 16;
    private const int PBKDF2_Iterations = 32767;
    private const string EncryptedFilePath = @"C:\Users\Billy\AppData\Roaming\PersonalVaultApplication\EncryptedFiles\Common\"; // TODO Move into base class
    private const string DecryptedFilePath = @"C:\Users\Billy\AppData\Roaming\PersonalVaultApplication\DecryptedFiles\Common\"; // TODO Move into base class

    private static byte[] GenerateSalt()
    {
        // Generate a 128-bit salt using a CSPRNG.
        SecureRandom rand = new();
        byte[] salt = new byte[PBKDF2_SaltSize];
        rand.NextBytes(salt);

        return salt;
    }

    private static byte[] GenerateKeyFromPassword(string password, byte[] salt)
    {
        // Create an instance of PBKDF2 and derive a key.
        Pkcs5S2ParametersGenerator pbkdf2 = new(new Sha256Digest());
        pbkdf2.Init(Encoding.UTF8.GetBytes(password), salt, PBKDF2_Iterations);
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
    /// 
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <returns>IV and Salt in that order</returns>
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
            var salt = buffer.Skip(AlgorithmnNonceSize).Take(PBKDF2_SaltSize).ToArray();

            return (iv, salt);
        }
        catch (UnauthorizedAccessException ex)
        {
            Debug.Print(ex.Message);
        }

        throw new Exception("Unable to get IV and Salt from EncryptedFile.");
    }

    /// <summary>Encrypt a file.</summary>
    /// <param name="sourceFilePath">The full path and name of the file to be encrypted.</param>
    /// <param name="password">The password for the encryption.</param>
    /// <returns>The encrypted files path</returns>
    public static string EncryptFile(string sourceFilePath, string password)
    {
        var initilisationVector = GenerateInitilisationVector();
        var salt = GenerateSalt();
        var key = GenerateKeyFromPassword(password, salt);
        var destinationFileName = EncryptFileName(Path.GetFileName(sourceFilePath), password);
        var destinationFilePath = EncryptedFilePath + destinationFileName + ".aes";

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

        // TODO Do this asyncronously.
        using var destination = new FileStream(@destinationFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        destination.BeginWrite(iVAndSalt, 0, iVAndSalt.Length, null, null);
        using var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write);
        using var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        source.CopyTo(cryptoStream);

        return destinationFilePath;
    }

    /// <summary>
    /// Decrypt a file from its path
    /// </summary>
    /// <remarks>NB: "Padding is invalid and cannot be removed." is the Universal CryptoServices error.  Make sure the password, salt and iterations are correct before getting nervous.</remarks>
    /// <returns>The path of the DecryptedFile</returns>
    /// <exception cref="ApplicationException"></exception>
    public static string DecryptFile(string sourceFilePath, string password, string destinationPath = DecryptedFilePath)
    {
        // Create an Aes object
        // with the specified key and IV.
        using Aes aesAlg = Aes.Create();
        var ivAndSalt = GetInitisationVectorAndSaltFromEncryptedFile(sourceFilePath);
        var iv = ivAndSalt.Item1;
        var salt = ivAndSalt.Item2;
        var key = GenerateKeyFromPassword(password, salt);
        var destinationFileName = DecryptFileName(Path.GetFileNameWithoutExtension(sourceFilePath), password);
        var destinationFileLocation = destinationPath + @"\" + destinationFileName;

        aesAlg.Key = key;
        aesAlg.IV = iv;

        ICryptoTransform transform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using FileStream destination = new FileStream(destinationFileLocation, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write);
        try
        {
            using FileStream source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            source.Position = 32; // Skip the IV and Salt porition of the stream
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
    /// Encrypts a string using AES Encryption using a given password as a key.
    /// </summary>
    /// <param name="plaintext">The text which is to be encrypted</param>
    /// <param name="password">The password to be used as a key</param>
    /// <returns>Encrypted string</returns>
    private static string EncryptFileName(string plaintext, string password)
    {
        var salt = GenerateSalt();
        var key = GenerateKeyFromPassword(password, salt);

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

    /// <summary>
    /// Decrypts a given file name using a given password as the key.
    /// </summary>
    /// <param name="encryptedFileName">A combiniation of the ciphertext, nonce and salt in the form of a file name</param>
    /// <param name="password">the password which was used as the key</param>
    /// <returns>The decrypted string</returns>
    public static string DecryptFileName(string encryptedFileName, string password)
    {
        // Decode the base64.
        byte[] ciphertextAndNonceAndSalt = Convert.FromBase64String(encryptedFileName.Replace("_", @"/")); // The .Replace is important here as / cannot be in file names in windows.
                                                                                                                            // There are several other illegal characters in windows but this is the only
        byte[] salt = new byte[PBKDF2_SaltSize];                                                                            // Retrieve the salt and ciphertextAndNonce. This process is inverted for Encrypting.
        byte[] ciphertextAndNonce = new byte[ciphertextAndNonceAndSalt.Length - PBKDF2_SaltSize];
        Array.Copy(ciphertextAndNonceAndSalt, 0, salt, 0, salt.Length);
        Array.Copy(ciphertextAndNonceAndSalt, salt.Length, ciphertextAndNonce, 0, ciphertextAndNonce.Length);

        // Create an instance of PBKDF2 and derive a key.
        Pkcs5S2ParametersGenerator pbkdf2 = new(new Sha256Digest());
        pbkdf2.Init(Encoding.UTF8.GetBytes(password), salt, PBKDF2_Iterations);
        byte[] key = ((KeyParameter)pbkdf2.GenerateDerivedMacParameters(AlgorithmKeySize * 8)).GetKey();

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

    public static bool VerifyPassword(string password)
    {
        var encryptedFile = Directory.GetFiles(EncryptedFilePath).FirstOrDefault();

        if (encryptedFile == null)
        {
            return true; // TODO Implement first time setup here
        }
        else
        {
            try
            {
                var encryptedFileName = Path.GetFileNameWithoutExtension(encryptedFile);
                var _ = DecryptFileName(encryptedFileName, password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}