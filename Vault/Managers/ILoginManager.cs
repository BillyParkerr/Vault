namespace Application.Managers;

public interface ILoginManager
{
    /// <summary>
    /// Sets the password for the application by encrypting a randomly generated string, setting the encryption key in the database,
    /// and updating the encryption password in the encryption manager.
    /// </summary>
    /// <param name="password">The password to set.</param>
    void SetPassword(string password);

    /// <summary>
    /// Verifies if the given password is correct by attempting to decrypt the stored encryption key.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <returns>True if the password is correct, otherwise false.</returns>
    bool VerifyPassword(string password);

    /// <summary>
    /// Changes the application password by decrypting the current encryption key with the old password, re-encrypting the key with the new password,
    /// updating the encryption key in the database, and setting the new password.
    /// </summary>
    /// <param name="newPassword">The new password to set.</param>
    /// <param name="oldPassword">The old password to validate.</param>
    /// <returns>True if the password change is successful, otherwise false.</returns>
    bool ChangePassword(string newPassword, string oldPassword);
}