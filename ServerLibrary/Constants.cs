using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary;

public static class Constants
{
    private static readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string appDirectoryPath = Path.Combine(appDataPath, "ZeroBank");

    public static readonly string ServerDirectoryPath = Path.Combine(appDirectoryPath, "WebAPI");
    public static readonly string AccountsDirectoryPath = Path.Combine(ServerDirectoryPath, "Accounts");
    public static readonly string PrivateKeysDirectoryPath = Path.Combine(ServerDirectoryPath, "Private Keys");
    public static readonly string UsersFilePath = Path.Combine(ServerDirectoryPath, "Users.json");
    public static readonly string UserPrivateKeysFilePath = Path.Combine(ServerDirectoryPath, "UserPrivateKeys.json");
}
