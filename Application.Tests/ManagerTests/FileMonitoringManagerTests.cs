using Application.Managers;
using Moq;

namespace Application.Tests.ManagerTests;

[TestFixture]
public class FileMonitoringManagerTests
{
    private Mock<IEncryptionManager> _encryptionManager;
    private Mock<IDatabaseManager> _databaseManager;
    private FileMonitoringManager _fileMonitoringManager;
    private TestHelper _testHelper;

    [SetUp]
    public void Setup()
    {
        _encryptionManager = new Mock<IEncryptionManager>();
        _databaseManager = new Mock<IDatabaseManager>();
        _fileMonitoringManager = new TestFileMonitoringManager(_encryptionManager.Object, _databaseManager.Object);
        _testHelper = new TestHelper();
    }

    // This is used to avoid files being opened when running the tests. It only overrides a specific method so all all other functionality should be equal to FileMonitoringManager.
    // Overriding methods should be kept to a minimum to ensure tests provide value.
    public class TestFileMonitoringManager : FileMonitoringManager
    {
        public TestFileMonitoringManager(IEncryptionManager encryptionManager, IDatabaseManager databaseManager)
            : base(encryptionManager, databaseManager)
        {
        }

        protected override void OpenGivenFile(string path)
        {
            // Do nothing
        }
    }

    [Test]
    public async Task FileMonitoringManager_UpdatesEncryptedFile_WhenMonitoredFileIsChanged()
    {
        // Arrange
        string testFile = _testHelper.CreateTextFile();
        string encryptedFilePath = testFile + ".encrypted";

        try
        {
            _fileMonitoringManager.Initilise(testFile, encryptedFilePath);

            // Modify the monitored file
            using (StreamWriter writer = File.AppendText(testFile))
            {
                writer.WriteLine("New line added.");
            }

            // Give some time for the watcher to process the change
            await Task.Delay(3000);

            // Verify that the appropriate methods were called to update the encrypted file
            _encryptionManager.Verify(x => x.EncryptFile(testFile, It.IsAny<string>()), Times.AtLeastOnce);
            _databaseManager.Verify(x => x.DeleteEncryptedFileByFilePath(encryptedFilePath), Times.AtLeastOnce);
            _databaseManager.Verify(x => x.SaveChanges(), Times.AtLeast(2));
            _databaseManager.Verify(x => x.AddEncryptedFile(It.IsAny<string>(), false), Times.AtLeastOnce);
        }
        finally
        {
            // Clean up
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
            if (File.Exists(encryptedFilePath))
            {
                File.Delete(encryptedFilePath);
            }
        }
    }
}