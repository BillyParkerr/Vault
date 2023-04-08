using Application.Managers;

namespace Application.Tests;

public class TestHelper
{
    private EncryptionManager _encryptionManager;
    private FileManager _fileManager;
    private DatabaseManager _databaseManager;
    private FileMonitoringManager _fileMonitoringManager;
    private AppSettings _appSettings;

    public TestHelper()
    {
        _databaseManager = new DatabaseManager();
        _databaseManager.SetSqliteDbContextIfNotExisits();

        _appSettings = new AppSettings();
        _encryptionManager = new EncryptionManager(_databaseManager);
        _fileMonitoringManager = new FileMonitoringManager(_encryptionManager, _databaseManager);
        _fileManager = new FileManager(_encryptionManager, _databaseManager, _fileMonitoringManager, _appSettings);
    }

    public string CreateTextFile()
    {
        // Create a unique file name based on the current date and time
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

        // Get the current directory and combine it with the file name
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        // Create a new text file and write some sample text to it
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("This is some sample text for testing purposes.");
            writer.WriteLine("It can be used to verify that file reading and writing functionality is working correctly.");
        }

        // Return the file path as a string
        return filePath;
    }
}