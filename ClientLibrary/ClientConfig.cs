using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public static class ClientConfig
{
    public static DataAccessor DataAccessor { get; set; } = new DataAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task SetUpClient()
    {
        if (!File.Exists(Constants.ParmsFilePath))
        {
            EncryptionHelper.Parms = await ApiAccessor.GetEncryptionParameters();
            (PublicKey publicKey, SecretKey secretKey, Serializable<RelinKeys> relinKeys) = EncryptionHelper.GenerateKeys();
            DataAccessor.SavePublicKey(publicKey);
            DataAccessor.SaveSecretKey(secretKey);
            DataAccessor.SaveRelinKeys(relinKeys);
            DataAccessor.SaveParms(EncryptionHelper.Parms);
        }
        else
        {
            EncryptionHelper.Parms = DataAccessor.LoadParms();
        }
    }
}
