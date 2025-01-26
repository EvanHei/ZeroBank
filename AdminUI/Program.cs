using AdminLibrary;
using AdminUI.Forms;

namespace AdminUI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        string username1 = "admin";
        string password1 = "admin";

        string username2 = "string";
        string password2 = "string";

        try
        {
            //await AdminConfig.AdminLogin(username1, password1);
            //await AdminConfig.AdminCreate(username2, password2);
            //await AdminConfig.AdminDelete(username2, password2);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}