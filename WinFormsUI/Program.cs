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
            await ClientConfig.SetUpClient();

            //Serializable<Ciphertext> ct1 = ClientConfig.EncryptionHelper.Encrypt(1234567890);
            //MemoryStream stream = new();
            //ct1.Save(stream);
            //stream.Seek(0, SeekOrigin.Begin);
            //Ciphertext ct2 = new();
            //ct2.Load(ClientConfig.EncryptionHelper.Context, stream);
            //long num = ClientConfig.EncryptionHelper.Decrypt(ct2);

            ApplicationConfiguration.Initialize();
            Application.Run(new Dashboard());
        }
    }
}