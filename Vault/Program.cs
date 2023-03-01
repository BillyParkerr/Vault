using Application.Managers;
using Application.Presenters;
using Application.Views;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Lifestyles;

namespace Application;

internal static class Program
{
    static Program()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        // Configure Dependency Injection Using SimpleInjecter
        container = new Container();
        container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

        container.Register<IDatabaseManager, DatabaseManager>(Lifestyle.Singleton);
        container.Register<IEncryptionManager, EncryptionManager>(Lifestyle.Transient);
        container.Register<IFileManager, FileManager>(Lifestyle.Transient);
        container.Register<IHomeView, HomeView>(Lifestyle.Transient);
        container.Register<ILoginView, LoginView>(Lifestyle.Transient);
        SuppressTransientWarning(typeof(IHomeView));
        SuppressTransientWarning(typeof(ILoginView));

        container.Verify();
    }

    /// <summary>
    /// Since we are suppressing this any container called that has this suppression will not dispose of itself.
    /// It is therefore very important that they are disposed of in code or they will always stay in memory.
    /// This has to be done as windows forms have the ability to dispose of themselves which is supposed to be the responsibily
    /// of SimpleInjector.
    /// </summary>
    /// <param name="serviceType"></param>
    private static void SuppressTransientWarning(Type serviceType)
    {
        Registration registration = container.GetRegistration(serviceType).Registration;

        registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Windows form suppression");
    }

    // Service Provider. Provides containers to be used throughout the application.
    public static readonly Container container;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        CreateApplicationFolders();
        //LoginInfomation.Password = "Password"; // TODO Remove this as its just for testing purposes!
        var databaseManager = container.GetInstance<IDatabaseManager>();
        bool registrationRequired = !databaseManager.IsEncryptionKeySet();

        if (registrationRequired)
        {

        }
        else
        {
            var encryptionManager = container.GetInstance<IEncryptionManager>();
            ILoginView loginView = container.GetInstance<ILoginView>();
            var loginViewPresenter = new LoginViewPresenter(loginView, encryptionManager);
            System.Windows.Forms.Application.Run((Form)loginView);
            if (loginViewPresenter.UserSuccessfullyAuthenticated)
            {
                RunHomeView();
            }
        }
    }

    private static void RunHomeView()
    {
        var homeView = container.GetInstance<IHomeView>();
        var fileManager = container.GetInstance<IFileManager>();
        var homeViewPresenter = new HomeViewPresenter(homeView, fileManager);
        System.Windows.Forms.Application.Run((Form)homeView);
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