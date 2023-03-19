using Windows.Security.Credentials.UI;

namespace Application.Managers;

public class WindowsHelloManager : IWindowsHelloManager
{
    private IFileManager fileManager;
    private IEncryptionManager encryptionManager;

    public WindowsHelloManager(IFileManager fileManager, IEncryptionManager encryptionManager)
    {
        this.fileManager = fileManager;
        this.encryptionManager = encryptionManager;
    }

    /// <summary>
    /// Creates a windows hello window for he user to authenticate themselves with.
    /// </summary>
    /// <param name="message">This is the message that will be displayed to the user.</param>
    /// <returns>true if authenticated, false is not</returns>
    public async Task<bool> AuthenticateWithWindowsHelloAsync(string message)
    {
        if (!IsWindowsHelloAvailable().Result)
        {
            return false;
        }

        // Request user authentication with Windows Hello
        UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync(message);
        if (consentResult == UserConsentVerificationResult.Verified)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool WindowsHelloLoginProcess()
    {
        bool authenticated = AuthenticateWithWindowsHelloAsync("Please authenticate to access the Vault.").Result;
        if (authenticated)
        {
            var password = fileManager.ReadAndReturnProtectedPassword();
            if (password != null)
            {
                encryptionManager.SetEncryptionPassword(password);
                return true;
            }
        }

        return false;
    }

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