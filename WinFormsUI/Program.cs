using ClientLibrary;
using ClientLibrary.Models;
using Microsoft.Research.SEAL;
using SharedLibrary;
using SharedLibrary.Models;
using WinFormsUI.Forms;

namespace WinFormsUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        string username1 = "Evan";
        string password1 = "password";
        string encryptionPassword1 = "p";

        try
        {
            //await ClientConfig.ApiAccessor.SignUp(username1, password1);
            await ClientConfig.ApiAccessor.Login(username1, password1);
            //await ClientConfig.CreateAccount("Account 1", AccountType.Checking, encryptionPassword1);
            //await ClientConfig.CreateAccount("Account 2", AccountType.Savings, encryptionPassword1);
            //await ClientConfig.AddTransaction(1, 5, encryptionPassword1); // balance is 5
            //await ClientConfig.AddTransaction(1, 10, encryptionPassword1); // balance is 15
            //await ClientConfig.AddTransaction(1, -10, encryptionPassword1); // balance is 5
            //await ClientConfig.AddTransaction(1, -10, encryptionPassword1); // balance is -5
            //long result = await ClientConfig.GetBalance(1, encryptionPassword1);
            //await ClientConfig.DeleteAccount(1);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new DashboardForm());
    }
}