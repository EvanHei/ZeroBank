using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public static class Constants
{
    private static readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string appDirectoryPath = Path.Combine(appDataPath, "ZeroBank");

    public static readonly string ClientDirectoryPath = Path.Combine(appDirectoryPath, "WinFormsUI");
    public static readonly string AccountsDirectoryPath = Path.Combine(ClientDirectoryPath, "Accounts");
    public static readonly string ParmsUrl = "https://localhost:7188/api/Parms";
    public static readonly string AccountsBaseUrl = "https://localhost:7188/api/Accounts";
    public static readonly string UsersBaseUrl = "https://localhost:7188/api/Users";
    public static readonly string GitHubReadmeUrl = "https://github.com/EvanHei/ZeroBank/blob/main/README.md";
}
