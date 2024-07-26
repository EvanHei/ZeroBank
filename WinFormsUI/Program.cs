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
            await ClientConfig.SetUpClient();

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}