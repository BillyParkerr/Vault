using Application.Views;

namespace Application;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        CreateApplicationFolders();

        LoginInfomation.Password = "Password"; // TODO Remove this as its just for testing purposes!

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.Run(new HomeView());
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