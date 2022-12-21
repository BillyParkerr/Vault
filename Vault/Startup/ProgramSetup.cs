namespace Application.Startup;

internal class ProgramSetup
{
    public ProgramSetup()
    {
        // TODO Add config here for getting information from config, maybe inject config into here as well.
    }

    public void Initialize()
    {
        CreateApplicationFolders();
    }

    private static void CreateApplicationFolders()
    {
        string programDirectory;
        // Create Application Folder
        if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
        {
            programDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonalVaultApplication");
            Directory.CreateDirectory(programDirectory);
        }
        else
        {
            // TODO add a custom directory setup.
            throw new Exception("No enviroment AppData set, cannot procceed with creating application folders!");
        }

        // Encryption Folders
        Directory.CreateDirectory(Path.Combine(programDirectory, "EncryptedFiles"));
        Directory.CreateDirectory(Path.Combine(programDirectory, @"EncryptedFiles\Common"));
        Directory.CreateDirectory(Path.Combine(programDirectory, @"EncryptedFiles\Custom"));

        // Decryption Folders
        Directory.CreateDirectory(Path.Combine(programDirectory, @"DecryptedFiles"));
        Directory.CreateDirectory(Path.Combine(programDirectory, @"DecryptedFiles\Common"));
        Directory.CreateDirectory(Path.Combine(programDirectory, @"DecryptedFiles\Temp"));

        // Config Folders
        Directory.CreateDirectory(Path.Combine(programDirectory, "Config"));
    }
}