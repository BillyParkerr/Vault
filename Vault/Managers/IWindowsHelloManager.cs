namespace Application.Managers;

public interface IWindowsHelloManager
{
    /// <summary>
    /// Authenticates the user with Windows Hello. This is done by showing the windows hello authentication window to the user
    /// </summary>
    /// <param name="message">The message to display during the authentication process.</param>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if the authentication was successful, otherwise false.</returns>
    Task<bool> AuthenticateWithWindowsHelloAsync(string message);

    /// <summary>
    /// Checks if Windows Hello is available on the device.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if Windows Hello is available, otherwise false.</returns>
    Task<bool> IsWindowsHelloAvailable();

    /// <summary>
    /// Authenticates the user using Windows Hello. If authenticated the protected password is read and the encryption password is set.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result of the task is true if the login process was successful, otherwise false.</returns>
    Task<bool> WindowsHelloLoginProcess();
}