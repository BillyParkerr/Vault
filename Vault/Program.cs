using Application.Forms;
using Application.Helpers;
using Application.Startup;

namespace Application;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        var setup = new ProgramSetup(); 
        setup.Initialize(); // Create application folders in AppData

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.Run(new HomePage());
    }
}