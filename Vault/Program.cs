using Application.Enums;
using Application.Managers;
using Application.Presenters;
using Application.Views;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        AppSettings appSettings = SetupAppSettings();
        container.RegisterInstance<AppSettings>(appSettings);

        // Register Views
        container.Register<IHomeView, HomeView>(Lifestyle.Transient);
        container.Register<ILoginView, LoginView>(Lifestyle.Transient);
        container.Register<IRegisterView, RegisterView>(Lifestyle.Transient);
        container.Register<IExportEncryptedFileView, ExportEncryptedFileView>(Lifestyle.Transient);
        container.Register<IImportEncryptedFileView, ImportEncryptedFileView>(Lifestyle.Transient);
        container.Register<IAuthenticationModeSelectionView, AuthenticationModeSelectionView>(Lifestyle.Transient);
        container.Register<IWindowsHelloRegisterView, WindowsHelloRegisterView>(Lifestyle.Transient);
        SuppressTransientWarning(typeof(IImportEncryptedFileView));
        SuppressTransientWarning(typeof(IExportEncryptedFileView));
        SuppressTransientWarning(typeof(IRegisterView));
        SuppressTransientWarning(typeof(IHomeView));
        SuppressTransientWarning(typeof(ILoginView));
        SuppressTransientWarning(typeof(IAuthenticationModeSelectionView));
        SuppressTransientWarning(typeof(IWindowsHelloRegisterView));

        // Register Managers
        container.Register<IWindowsHelloManager, WindowsHelloManager>(Lifestyle.Singleton);
        container.Register<IEncryptionManager, EncryptionManager>(Lifestyle.Singleton);
        container.Register<IDatabaseManager, DatabaseManager>(Lifestyle.Singleton);
        container.Register<IFileMonitoringManager, FileMonitoringManager>(Lifestyle.Transient);
        container.Register<IFileManager, FileManager>(Lifestyle.Transient);
        container.Register<ILoginManager, LoginManager>(Lifestyle.Transient);

        container.Verify();
    }

    /// <summary>
    /// Since we are suppressing this, any container called that has this suppression will not dispose of itself.
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
        var fileManager = container.GetInstance<IFileManager>();
        fileManager.CleanupTempFiles();

        var databaseManager = container.GetInstance<IDatabaseManager>();
        var windowsHelloManager = container.GetInstance<IWindowsHelloManager>();
        var appSettings = container.GetInstance<AppSettings>();

        if (appSettings.AuthenticationMethod == AuthenticationMethod.None)
        {
            // This method will get the user to choose a authentication method.
            // This will then be set in the appSettings
            GetAuthenticationMethodFromUser(windowsHelloManager, appSettings);
        }

        if (appSettings.AuthenticationMethod == AuthenticationMethod.Password)
        {
            RunApplicationInPasswordMode(databaseManager);
        }
        else if (appSettings.AuthenticationMethod == AuthenticationMethod.WindowsHello)
        {
            RunApplicationInWindowsHelloModeAsync(databaseManager, fileManager, windowsHelloManager).GetAwaiter().GetResult();
        }
    }

    private static async Task RunApplicationInWindowsHelloModeAsync(IDatabaseManager databaseManager, IFileManager fileManager, IWindowsHelloManager windowsHelloManager)
    {
        bool isPasswordSet = databaseManager.IsEncryptionKeySet() && fileManager.ProtectedPasswordExists();
        if (!isPasswordSet)
        {
            IWindowsHelloRegisterView windowsHelloRegisterView = container.GetInstance<IWindowsHelloRegisterView>();
            ILoginManager loginManager = container.GetInstance<ILoginManager>();
            var windowsHelloRegisterViewPresenter = new WindowsHelloRegisterViewPresenter(windowsHelloRegisterView, windowsHelloManager, loginManager);
            System.Windows.Forms.Application.Run((Form)windowsHelloRegisterView);
            if (windowsHelloRegisterViewPresenter.UserSuccessfullyRegistered)
            {
                RunHomeView();
            }
            else
            {
                // TODO Create a fallback where the application asks the user if its okay to swap to Password mode.
            }
        }
        else
        {
            bool loggedIn = await windowsHelloManager.WindowsHelloLoginProcess();
            if (loggedIn)
            {
                RunHomeView();
            }
            else
            {
                MessageBox.Show("The application couldn't authenticate you using windows hello. \n Please login using your backup password.");
                RunApplicationInPasswordMode(databaseManager);
            }
        }
    }

    private static void RunApplicationInPasswordMode(IDatabaseManager databaseManager)
    {
        bool isPasswordSet = databaseManager.IsEncryptionKeySet();
        if (!isPasswordSet)
        {
            IRegisterView registerView = container.GetInstance<IRegisterView>();
            ILoginManager passwordLoginManager = container.GetInstance<ILoginManager>();
            var registerViewPresenter = new RegistrationViewPresenter(passwordLoginManager, registerView);
            System.Windows.Forms.Application.Run((Form)registerView);
            if (registerViewPresenter.UserSuccessfullyRegistered)
            {
                RunHomeView();
            }
        }
        else
        {
            ILoginView loginView = container.GetInstance<ILoginView>();
            ILoginManager passwordLoginManager = container.GetInstance<ILoginManager>();
            IEncryptionManager encryptionManager = container.GetInstance<IEncryptionManager>();
            var loginViewPresenter = new LoginViewPresenter(loginView, passwordLoginManager, encryptionManager);
            System.Windows.Forms.Application.Run((Form)loginView);
            if (loginViewPresenter.UserSuccessfullyAuthenticated)
            {
                RunHomeView();
            }
        }
    }

    private static void GetAuthenticationMethodFromUser(IWindowsHelloManager windowsHelloManager, AppSettings appSettings)
    {
        // Check for windows hello availablility
        bool isWindowsHelloAvailable = windowsHelloManager.IsWindowsHelloAvailable().Result;
        if (isWindowsHelloAvailable)
        {
            IAuthenticationModeSelectionView view = container.GetInstance<IAuthenticationModeSelectionView>();
            AuthenticationModeSelectionViewPresenter presenter = new AuthenticationModeSelectionViewPresenter(view, appSettings);
            System.Windows.Forms.Application.Run((Form)view);
        }
        else
        {
            // If windows hello is not available the user will be forced to use a password.
            appSettings.AuthenticationMethod = AuthenticationMethod.Password;
            UpdateAppSettings(appSettings);
        }
    }

    private static void RunHomeView()
    {
        var homeView = container.GetInstance<IHomeView>();
        var fileManager = container.GetInstance<IFileManager>();
        var databaseManager = container.GetInstance<IDatabaseManager>();
        var homeViewPresenter = new HomeViewPresenter(homeView, fileManager, databaseManager);
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
        Directory.CreateDirectory(DirectoryPaths.AppSettingsPath);
    }

    private static AppSettings SetupAppSettings()
    {
        // Load the configuration file
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        // Bind the configuration settings to the AppSettings class
        var appSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(appSettings);

        return appSettings;
    }

    public static void UpdateAppSettings(AppSettings appSettings, string configFilePath = "appsettings.json")
    {
        // Read the configuration file into a JObject
        var configJson = File.ReadAllText(configFilePath);
        var config = JObject.Parse(configJson);

        // Update the settings in the JObject
        string appSettingsJson = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
        var appSettingsJObject = JObject.Parse(appSettingsJson);
        config["AppSettings"] = appSettingsJObject;

        // Write the updated JObject back to the configuration file
        File.WriteAllText(configFilePath, config.ToString(Formatting.Indented));
    }
}