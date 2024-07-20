using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public static class Constants
{
    private static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string appDirectoryPath = Path.Combine(appDataPath, "ZeroBank");

    public static readonly string ClientDirectoryPath = Path.Combine(appDirectoryPath, "WinFormsUI");
    public static readonly string PublicKeyFilePath = Path.Combine(ClientDirectoryPath, "public_key.bin");
    public static readonly string SecretKeyFilePath = Path.Combine(ClientDirectoryPath, "secret_key.bin");
    public static readonly string RelinKeysFilePath = Path.Combine(ClientDirectoryPath, "relin_keys.bin");
    public static readonly string ParmsFilePath = Path.Combine(ClientDirectoryPath, "parms.bin");
}
