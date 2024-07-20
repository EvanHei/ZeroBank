using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ClientLibrary;

public class DataAccessor
{
    public DataAccessor()
    {
        Directory.CreateDirectory(Constants.ClientDirectoryPath);
    }

    public void SavePublicKey(PublicKey publicKey)
    {
        if (publicKey == null)
        {
            throw new ArgumentNullException(nameof(publicKey), "Public key cannot be null.");
        }

        using FileStream stream = new(Constants.PublicKeyFilePath, FileMode.Append, FileAccess.Write);
        publicKey.Save(stream);
    }

    public PublicKey? LoadPublicKey()
    {
        if (!File.Exists(Constants.PublicKeyFilePath))
        {
            return null;
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            return null;
        }

        using FileStream stream = new(Constants.PublicKeyFilePath, FileMode.Open, FileAccess.Read);
        PublicKey publicKey = new();
        publicKey.Load(ClientConfig.EncryptionHelper.Context, stream);
        return publicKey;
    }

    public void SaveSecretKey(SecretKey secretKey)
    {
        if (secretKey == null)
        {
            throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null.");
        }

        using FileStream stream = new(Constants.SecretKeyFilePath, FileMode.Append, FileAccess.Write);
        secretKey.Save(stream);
    }

    public SecretKey? LoadSecretKey()
    {
        if (!File.Exists(Constants.SecretKeyFilePath))
        {
            return null;
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            return null;
        }

        using FileStream stream = new(Constants.SecretKeyFilePath, FileMode.Open, FileAccess.Read);
        SecretKey secretKey = new();
        secretKey.Load(ClientConfig.EncryptionHelper.Context, stream);
        return secretKey;
    }

    public void SaveRelinKeys(Serializable<RelinKeys> relinKeys)
    {
        if (relinKeys == null)
        {
            throw new ArgumentNullException(nameof(relinKeys), "Relinearizaion keys cannot be null.");
        }

        using FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Append, FileAccess.Write);
        relinKeys.Save(stream);
    }

    public RelinKeys? LoadRelinKeys()
    {
        if (!File.Exists(Constants.RelinKeysFilePath))
        {
            return null;
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            return null;
        }

        using FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Open, FileAccess.Read);
        RelinKeys relinKeys = new();
        relinKeys.Load(ClientConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }

    public void SaveParms(EncryptionParameters parms)
    {
        if (parms == null)
        {
            throw new ArgumentNullException(nameof(parms), "Encryption parameters cannot be null.");
        }

        using FileStream stream = new(Constants.ParmsFilePath, FileMode.Append, FileAccess.Write);
        parms.Save(stream);
    }

    public EncryptionParameters? LoadParms()
    {
        if (!File.Exists(Constants.ParmsFilePath))
        {
            return null;
        }

        using FileStream stream = new(Constants.ParmsFilePath, FileMode.Open, FileAccess.Read);
        EncryptionParameters parms = new();
        parms.Load(stream);
        return parms;
    }
}