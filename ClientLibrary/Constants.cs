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
    public static readonly string PublicKeyFilePath = Path.Combine(ClientDirectoryPath, "public_key.bin");
    public static readonly string SecretKeyFilePath = Path.Combine(ClientDirectoryPath, "secret_key.bin");
    public static readonly string ParmsFilePath = Path.Combine(ClientDirectoryPath, "parms.bin");
    public static readonly string ParmsUrl = "https://localhost:7188/parms";
    public static readonly string AccountsBaseUrl = "https://localhost:7188/accounts";
}
