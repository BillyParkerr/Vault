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
        container.Register<IEncryptionManager, EncryptionManager>(Lifestyle.Singleton);
        container.Register<IFileMonitoringManager, FileMonitoringManager>(Lifestyle.Transient);
        container.Register<IFileManager, FileManager>(Lifestyle.Transient);
        container.Register<IHomeView, HomeView>(Lifestyle.Transient);
        container.Register<ILoginView, LoginView>(Lifestyle.Transient);
        container.Register<IRegisterView, RegisterView>(Lifestyle.Transient);
        container.Register<IExportEncryptedFileView, ExportEncryptedFileView>(Lifestyle.Transient);
        container.Register<IImportEncryptedFileView, ImportEncryptedFileView>(Lifestyle.Transient);
        SuppressTransientWarning(typeof(IImportEncryptedFileView));
        SuppressTransientWarning(typeof(IExportEncryptedFileView));
        SuppressTransientWarning(typeof(IRegisterView));
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
        var databaseManager = container.GetInstance<IDatabaseManager>();
        var encryptionManager = container.GetInstance<IEncryptionManager>();
        bool registrationRequired = !databaseManager.IsEncryptionKeySet();

        if (registrationRequired)
        {
            IRegisterView registerView = container.GetInstance<IRegisterView>();
            var registerViewPresenter = new RegistrationViewPresenter(encryptionManager, registerView);
            System.Windows.Forms.Application.Run((Form)registerView);
            if (registerViewPresenter.UserSuccessfullyRegistered)
            {
                RunHomeView();
            }
        }
        else
        {
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
        string programDirectory = DirectoryPaths.ProgramDirectory;

        // Encryption Folders
        Directory.CreateDirectory(DirectoryPaths.EncryptedFilesDirectory);
        Directory.CreateDirectory(DirectoryPaths.EncryptedFilesCommonDirectory);
        Directory.CreateDirectory(DirectoryPaths.EncryptedFilesCustomDirectory);

        // Decryption Folders
        Directory.CreateDirectory(DirectoryPaths.DecryptedFilesDirectory);
        Directory.CreateDirectory(DirectoryPaths.DecryptedFilesCommonDirectory);
        Directory.CreateDirectory(DirectoryPaths.DecryptedFilesTempDirectory);

        // Config Folders
        Directory.CreateDirectory(DirectoryPaths.ConfigDirectory);
    }
}