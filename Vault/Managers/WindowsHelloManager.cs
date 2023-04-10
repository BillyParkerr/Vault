using Windows.Security.Credentials.UI;

namespace Application.Managers;

public class WindowsHelloManager : IWindowsHelloManager
{
    private readonly IFileManager _fileManager;
    private readonly IEncryptionManager _encryptionManager;

    public WindowsHelloManager(IFileManager fileManager, IEncryptionManager encryptionManager)
    {
        _fileManager = fileManager;
        _encryptionManager = encryptionManager;
    }

    /// <summary>
    /// Authenticates the user with Windows Hello. This is done by showing the windows hello authentication window to the user
    /// </summary>
    /// <param name="message">The message to display during the authentication process.</param>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if the authentication was successful, otherwise false.</returns>
    public async Task<bool> AuthenticateWithWindowsHelloAsync(string message)
    {
        if (!await IsWindowsHelloAvailable().ConfigureAwait(false))
        {
            return false;
        }

        // Request user authentication with Windows Hello
        UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync(message).AsTask().ConfigureAwait(false);
        if (consentResult == UserConsentVerificationResult.Verified)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Authenticates the user using Windows Hello. If authenticated the protected password is read and the encryption password is set.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if the login process was successful, otherwise false.</returns>
    public async Task<bool> WindowsHelloLoginProcess()
    {
        bool authenticated = await AuthenticateWithWindowsHelloAsync("Please authenticate to access the Vault.").ConfigureAwait(false);
        if (authenticated)
        {
            var password = _fileManager.ReadAndReturnProtectedPassword();
            if (password != null)
            {
                _encryptionManager.SetEncryptionPassword(password);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if Windows Hello is available on the device.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if Windows Hello is available, otherwise false.</returns>
    public async Task<bool> IsWindowsHelloAvailable()
    {
        // Check if Windows Hello is available on the device
        var consentAvailability = await UserConsentVerifier.CheckAvailabilityAsync();
        if (consentAvailability != UserConsentVerifierAvailability.Available)
        {
            return false;
        }

        return true;
    }
}