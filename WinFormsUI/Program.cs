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
        static void Main()
        {
            // TODO: test and then clean up
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDirectoryPath = Path.Combine(appDataPath, Constants.AppDirectoryName);
            string clientDirectoryPath = Path.Combine(appDirectoryPath, Constants.ClientDirectoryName);
            string relinKeysFilePath = Path.Combine(clientDirectoryPath, Constants.RelinKeysFileName);

            if (!File.Exists(relinKeysFilePath))
            {
                EncryptionParameters parms = ClientConfig.ApiAccessor.GetEncryptionParameters();
                ClientConfig.EncryptionHelper.Parms = parms;
                (PublicKey publicKey, SecretKey secretKey, Serializable<RelinKeys> relinKeys) = ClientConfig.EncryptionHelper.GenerateKeys();
                ClientConfig.DataAccessor.SavePublicKey(publicKey);
                ClientConfig.DataAccessor.SaveSecretKey(secretKey);
                ClientConfig.DataAccessor.SaveRelinKeys(relinKeys);
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}