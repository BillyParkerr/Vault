using System.Diagnostics;

namespace Application.Managers;

/// <summary>
/// This class is used when a file needs to be monitered.
///
/// When the monitered file is edited this class will first remove the previous encrypted copy.
/// It will then reencrypt the new copy and add it to the vault
///
/// It is important to note that an instance of this class can only moniter a single file.
/// Multiple Initilise calls will simply replace the file being monitered.
/// </summary>
public class FileMonitoringManager : IFileMonitoringManager
{
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly IEncryptionManager _encryptionManager;
    private readonly IDatabaseManager _databaseManager;
    private string _encryptedFilePath;

    public FileMonitoringManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager)
    {
        _encryptionManager = encryptionManager;
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Initializes the file monitoring manager by setting the encrypted file path, opening the file to monitor, and setting up a FileSystemWatcher to watch for changes in the file.
    /// </summary>
    /// <param name="fileToMonitorPath">The path of the file to monitor.</param>
    /// <param name="encryptedFilePath">The path of the encrypted file.</param>
    public void Initilise(string fileToMonitorPath, string encryptedFilePath)
    {
        _encryptedFilePath = encryptedFilePath;
        string fileDirectory = Path.GetDirectoryName(fileToMonitorPath);

        if (string.IsNullOrWhiteSpace(fileToMonitorPath) || string.IsNullOrWhiteSpace(encryptedFilePath) || string.IsNullOrWhiteSpace(fileDirectory))
        {
            return;
        }

        OpenGivenFile(fileToMonitorPath);

        // Moniter the file for any changes
        var watcher = new FileSystemWatcher();
        watcher.Path = fileDirectory;
        watcher.Filter = Path.GetFileName(fileToMonitorPath);
        watcher.EnableRaisingEvents = true;
        watcher.Changed += OnFileChanged;
    }

    /// <summary>
    ///  This event is raised when the monitered file is changed.
    ///  The change is detected based upon the date modified flag from the os.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        await _semaphore.WaitAsync();

        try
        {
            // When the file is changed / modified we should refresh the instance that is within the vault.
            // This means first removing the file from the vault then reencrypting / readding it.

            // Delete the encrypted file that we have created earlier.
            File.Delete(_encryptedFilePath);
            _databaseManager.DeleteEncryptedFileByFilePath(_encryptedFilePath);
            _databaseManager.SaveChanges(); // Both SaveChanges calls are required as otherwise issues with threading and concurrent DbContext calls can happen.

            // Readd the new modified file to the vault.
            // Encrypt File
            string newEncryptedFilePath = _encryptionManager.EncryptFile(e.FullPath);
            _databaseManager.AddEncryptedFile(newEncryptedFilePath);
            _databaseManager.SaveChanges();
            _encryptedFilePath = newEncryptedFilePath;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected virtual void OpenGivenFile(string filePath)
    {
        // Open the file
        using Process fileopener = new();
        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + filePath + "\"";
        fileopener.Start();
    }
}