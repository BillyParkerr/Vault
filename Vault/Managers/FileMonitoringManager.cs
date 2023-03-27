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
    private readonly SemaphoreSlim semaphore = new(1);
    private readonly IEncryptionManager EncryptionManager;
    private readonly IDatabaseManager DatabaseManager;
    private string EncryptedFilePath;

    public FileMonitoringManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager)
    {
        EncryptionManager = encryptionManager;
        DatabaseManager = databaseManager;
    }

    public void Initilise(string fileToMonitorPath, string encryptedFilePath)
    {
        if(string.IsNullOrWhiteSpace(fileToMonitorPath) || string.IsNullOrWhiteSpace(encryptedFilePath))
        {
            return;
        }

        EncryptedFilePath = encryptedFilePath;

        // Open the file
        using Process fileopener = new();

        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + fileToMonitorPath + "\"";
        fileopener.Start();

        // Moniter the file for any changes
        var watcher = new FileSystemWatcher();
        watcher.Path = Path.GetDirectoryName(fileToMonitorPath);
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
        await semaphore.WaitAsync();

        try
        {
            // When the file is changed / modified we should refresh the instance that is within the vault.
            // This means first removing the file from the vault then reencrypting / readding it.

            // Delete the encrypted file that we have created earlier.
            File.Delete(EncryptedFilePath);
            DatabaseManager.DeleteEncryptedFileByFilePath(EncryptedFilePath);
            DatabaseManager.SaveChanges(); // Both SaveChanges calls are required as otherwise issues with threading and concurrent DbContext calls can happen.

            // Readd the new modified file to the vault.
            // Encrypt File
            string newEncryptedFilePath = EncryptionManager.EncryptFile(e.FullPath);
            DatabaseManager.AddEncryptedFile(newEncryptedFilePath, false);
            DatabaseManager.SaveChanges();
            EncryptedFilePath = newEncryptedFilePath;
        }
        finally
        {
            semaphore.Release();
        }
    }
}