namespace Application.Managers;

public interface IFileMonitoringManager
{
    void Initilise(string fileToMonitor, string encryptedFilePath);
}