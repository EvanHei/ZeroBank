using ClientLibrary;
using Microsoft.Research.SEAL;
using SharedLibrary;

namespace WinFormsUI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        string password = "Password123";

        try
        {
            await ClientConfig.CreateAccount("Test", AccountType.Checking, password);
            await ClientConfig.AddTransactionById(1, 15, password);
            await ClientConfig.AddTransactionById(1, -20, password);
            long result = await ClientConfig.GetBalanceById(1, password);
            List<Account> accounts = await ClientConfig.ApiAccessor.GetAccounts();
            await ClientConfig.DeleteAccountById(1);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new Dashboard());
    }
}