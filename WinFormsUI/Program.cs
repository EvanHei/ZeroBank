using ClientLibrary;
using Microsoft.Research.SEAL;
using SharedLibrary;

namespace WinFormsUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            //await ClientConfig.CreateAccount("Test", AccountType.Checking);
            //await ClientConfig.AddTransactionById(15, 1);
            //await ClientConfig.AddTransactionById(-20, 1);
            //long result = await ClientConfig.GetBalanceById(1);

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}