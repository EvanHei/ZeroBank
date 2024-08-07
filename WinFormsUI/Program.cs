using ClientLibrary;
using ClientLibrary.Models;
using Microsoft.Research.SEAL;
using SharedLibrary;
using SharedLibrary.Models;

namespace WinFormsUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        string username1 = "Evan";
        string password1 = "password";
        string encryptionPassword1 = "Password123";

        try
        {
            // user 1
            //await ClientConfig.ApiAccessor.SignUp(username1, password1);
            await ClientConfig.ApiAccessor.Login(username1, password1);
            //await ClientConfig.CreateAccount("Evan's account", AccountType.Checking, encryptionPassword1);
            //await ClientConfig.AddTransaction(1, 15, encryptionPassword1);
            //await ClientConfig.AddTransaction(1, -20, encryptionPassword1);
            //long result = await ClientConfig.GetBalance(1, encryptionPassword1);
            //List<Account> accounts = await ClientConfig.ApiAccessor.GetAccounts();
            //List<PlaintextTransaction> plainTransactions = await ClientConfig.GetPlaintextTransactions(1, encryptionPassword1);
            //await ClientConfig.DeleteAccount(1);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new Dashboard());
    }
}