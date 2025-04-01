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
        Credentials adminCredentials_2 = new("string", "string");

        try
        {
            await AdminConfig.AdminLogin(adminCredentials_1);
            //await AdminConfig.AdminCreate(adminCredentials_2);
            //await AdminConfig.AdminDelete(adminCredentials_1);
            List<Account> accounts = await AdminConfig.ApiAccessor.GetUserAccounts();

            // must be 1 account to work
            ulong value = AdminConfig.LoadPlainModulus(accounts[0]);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}