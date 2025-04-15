using AdminLibrary;
using AdminUI.Forms;
using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System.Security.Principal;

namespace AdminUI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        Credentials adminCredentials_1 = new("admin", "admin");

        try
        {
            await AdminConfig.AdminLogin(adminCredentials_1);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}