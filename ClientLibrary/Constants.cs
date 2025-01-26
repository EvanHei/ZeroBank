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
    public static readonly string GitHubReadmeUrl = "https://github.com/EvanHei/ZeroBank/blob/main/README.md";

    public static readonly string UserLoginUrl = "https://localhost:7188/api/User/login";
    public static readonly string UserSignUpUrl = "https://localhost:7188/api/User/signup";

    public static readonly string AccountBaseUrl = "https://localhost:7188/api/Account";
    public static readonly string AccountPartialAccountUrl = "https://localhost:7188/api/Account/post-partial";
    public static readonly string AccountFullAccountUrl = "https://localhost:7188/api/Account/post-full";
    public static readonly string AccountTransactionUrl = "https://localhost:7188/api/Account/transaction";
    public static readonly string AccountCloseUrl = "https://localhost:7188/api/Account/close";
}
