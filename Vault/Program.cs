using Application.Enums;
using Application.Managers;
using Application.Presenters;
using Application.Views.Forms;
using Application.Views.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Lifestyles;

namespace Application;

internal static class Program
{
    // Service Provider. Provides containers to be used throughout the application.
    public static readonly Container Container = new();

    static Program()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        ConfigureDependencyInjection();
    }

    private static void ConfigureDependencyInjection()
    {
        // Configure Dependency Injection Using SimpleInjecter
        Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

        AppSettings appSettings = SetupAppSettings();
        Container.RegisterInstance<AppSettings>(appSettings);

        // Register Views
        Container.Register<IHomeView, HomeView>(Lifestyle.Transient);
        Container.Register<ILoginView, LoginView>(Lifestyle.Transient);
        Container.Register<IRegisterView, RegisterView>(Lifestyle.Transient);
        Container.Register<IExportEncryptedFileView, ExportEncryptedFileView>(Lifestyle.Transient);
        Container.Register<IImportEncryptedFileView, ImportEncryptedFileView>(Lifestyle.Transient);
        Container.Register<IAuthenticationModeSelectionView, AuthenticationModeSelectionView>(Lifestyle.Transient);
        Container.Register<IWindowsHelloRegisterView, WindowsHelloRegisterView>(Lifestyle.Transient);
        Container.Register<ISettingsView, SettingsView>(Lifestyle.Transient);
        Container.Register<IChangePasswordView, ChangePasswordView>(Lifestyle.Transient);
        Container.Register<IVerifyPasswordView, VerifyPasswordView>(Lifestyle.Transient);

        SuppressTransientWarning(typeof(IImportEncryptedFileView));
        SuppressTransientWarning(typeof(IExportEncryptedFileView));
        SuppressTransientWarning(typeof(IRegisterView));
        SuppressTransientWarning(typeof(IHomeView));
        SuppressTransientWarning(typeof(ILoginView));
        SuppressTransientWarning(typeof(IAuthenticationModeSelectionView));
        SuppressTransientWarning(typeof(IWindowsHelloRegisterView));
        SuppressTransientWarning(typeof(ISettingsView));
        SuppressTransientWarning(typeof(IChangePasswordView));
        SuppressTransientWarning(typeof(IVerifyPasswordView));

        // Register Managers
        Container.Register<IEncryptionManager, EncryptionManager>(Lifestyle.Singleton);
        Container.Register<IDatabaseManager, DatabaseManager>(Lifestyle.Singleton);
        Container.Register<IWindowsHelloManager, WindowsHelloManager>(Lifestyle.Transient);
        Container.Register<IFileMonitoringManager, FileMonitoringManager>(Lifestyle.Transient);
        Container.Register<IFileManager, FileManager>(Lifestyle.Transient);
        Container.Register<ILoginManager, LoginManager>(Lifestyle.Transient);
        Container.Register<IPresenterManager, PresenterManager>(Lifestyle.Transient);

        Container.Verify();
    }

    /// <summary>
    /// Since we are suppressing this, any container called that has this suppression will not dispose of itself.
    /// It is therefore very important that they are disposed of in code or they will always stay in memory.
    /// This has to be done as windows forms have the ability to dispose of themselves which is supposed to be the responsibility
    /// of SimpleInjector.
    /// </summary>
    /// <param name="serviceType"></param>
    private static void SuppressTransientWarning(Type serviceType)
    {
        Registration registration = Container.GetRegistration(serviceType).Registration;

        registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Windows form suppression");
    }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        CreateApplicationFolders();
        var fileManager = Container.GetInstance<IFileManager>();
        fileManager.CleanupTempFiles();

        var databaseManager = Container.GetInstance<IDatabaseManager>();
        databaseManager.SetSqliteDbContextIfNotExisits();

        var windowsHelloManager = Container.GetInstance<IWindowsHelloManager>();
        var appSettings = Container.GetInstance<AppSettings>();

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
            IWindowsHelloRegisterView windowsHelloRegisterView = Container.GetInstance<IWindowsHelloRegisterView>();
            var presenterManager = Container.GetInstance<IPresenterManager>();
            var windowsHelloRegisterViewPresenter = presenterManager.GetWindowsHelloRegisterViewPresenter(windowsHelloRegisterView);
            System.Windows.Forms.Application.Run((Form)windowsHelloRegisterView);
            if (windowsHelloRegisterViewPresenter.UserSuccessfullyRegistered)
            {
                RunHomeView();
            }
            else
            {
                return;
            }
        }
        else
        {
            bool loggedIn = await windowsHelloManager.WindowsHelloLoginProcess().ConfigureAwait(false);
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
            IRegisterView registerView = Container.GetInstance<IRegisterView>();
            ILoginManager passwordLoginManager = Container.GetInstance<ILoginManager>();
            var registerViewPresenter = new RegistrationViewPresenter(passwordLoginManager, registerView);
            System.Windows.Forms.Application.Run((Form)registerView);
            if (registerViewPresenter.UserSuccessfullyRegistered)
            {
                RunHomeView();
            }
        }
        else
        {
            ILoginView loginView = Container.GetInstance<ILoginView>();
            ILoginManager passwordLoginManager = Container.GetInstance<ILoginManager>();
            IEncryptionManager encryptionManager = Container.GetInstance<IEncryptionManager>();
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
            IAuthenticationModeSelectionView view = Container.GetInstance<IAuthenticationModeSelectionView>();
            AuthenticationModeSelectionViewPresenter presenter = new(view, appSettings);
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
        var homeView = Container.GetInstance<IHomeView>();
        var presenterManager = Container.GetInstance<IPresenterManager>();

        var homeViewPresenter = presenterManager.GetHomeViewPresenter(homeView);
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

        // Set the DefaultDownloadLocation to the user's desktop folder if it is empty, null or whitespace.
        if (string.IsNullOrWhiteSpace(appSettings.DefaultDownloadLocation))
        {
            appSettings.DefaultDownloadLocation = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

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