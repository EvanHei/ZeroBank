using ClientLibrary;
using Microsoft.Research.SEAL;
using SharedLibrary;

namespace WinFormsUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        string username1 = "Evan";
        string password1 = "password";
        string encryptionPassword1 = "Password123";

        string username2 = "Ian";
        string password2 = "p";
        string encryptionPassword2 = "k";

        try
        {
            // user 1
            await ClientConfig.ApiAccessor.SignUp(username1, password1);
            await ClientConfig.ApiAccessor.Login(username1, password1);
            var p = await ClientConfig.ApiAccessor.GetEncryptionParameters();
            await ClientConfig.CreateAccount("Evan's account", AccountType.Checking, encryptionPassword1);
            await ClientConfig.AddTransactionById(1, 15, encryptionPassword1);
            await ClientConfig.AddTransactionById(1, -20, encryptionPassword1);
            long result = await ClientConfig.GetBalanceById(1, encryptionPassword1);
            List<Account> accounts = await ClientConfig.ApiAccessor.GetAccounts();

            // user 2
            await ClientConfig.ApiAccessor.SignUp(username2, password2);
            await ClientConfig.ApiAccessor.Login(username2, password2);
            p = await ClientConfig.ApiAccessor.GetEncryptionParameters();
            await ClientConfig.CreateAccount("Ian's account", AccountType.Checking, encryptionPassword2);
            await ClientConfig.AddTransactionById(2, 15, encryptionPassword2);
            await ClientConfig.AddTransactionById(2, -20, encryptionPassword2);
            result = await ClientConfig.GetBalanceById(2, encryptionPassword2);
            accounts = await ClientConfig.ApiAccessor.GetAccounts();

            await ClientConfig.DeleteAccountById(2);
            await ClientConfig.ApiAccessor.Login(username1, password1);
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