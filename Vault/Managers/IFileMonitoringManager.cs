namespace Application.Managers;

public interface IFileMonitoringManager
{
    /// <summary>
    /// Initializes the file monitoring manager by setting the encrypted file path, opening the file to monitor, and setting up a FileSystemWatcher to watch for changes in the file.
    /// </summary>
    /// <param name="fileToMonitorPath">The path of the file to monitor.</param>
    /// <param name="encryptedFilePath">The path of the encrypted file.</param>
    void Initilise(string fileToMonitorPath, string encryptedFilePath);
}