using ClientLibrary;
using Microsoft.Research.SEAL;

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
            ClientConfig.SetUpClient();

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}