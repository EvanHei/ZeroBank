using SharedLibrary;
using SharedLibrary.Models;
using Microsoft.Research.SEAL;
using ClientUI.Forms;

namespace ClientUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        //string username1 = "Evan";
        //string password1 = "password";

        try
        {
            //await ClientConfig.SignUp(username1, password1);
            //await ClientConfig.ApiAccessor.Login(username1, password1);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}