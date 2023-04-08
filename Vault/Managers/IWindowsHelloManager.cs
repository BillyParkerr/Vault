namespace Application.Managers;

public interface IWindowsHelloManager
{
    Task<bool> AuthenticateWithWindowsHelloAsync(string message);
    Task<bool> IsWindowsHelloAvailable();
    Task<bool> WindowsHelloLoginProcess();
}