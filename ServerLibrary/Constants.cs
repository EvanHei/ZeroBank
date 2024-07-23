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
    public static readonly string RelinKeysFilePath = Path.Combine(ServerDirectoryPath, "relin_keys.bin");
    public static readonly string AccountsFilePath = Path.Combine(ServerDirectoryPath, "accounts.json");
}
