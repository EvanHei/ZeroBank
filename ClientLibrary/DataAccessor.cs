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
    private string ClientDirectoryPath { get; set; }

    public DataAccessor()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDirectoryPath = Path.Combine(appDataPath, Constants.AppDirectoryName);
        ClientDirectoryPath = Path.Combine(appDirectoryPath, Constants.ClientDirectoryName);
        Directory.CreateDirectory(ClientDirectoryPath);
    }

    // TODO: test SavePublicKey
    public void SavePublicKey(PublicKey publicKey)
    {
        if (publicKey == null)
        {
            throw new ArgumentNullException(nameof(publicKey), "Public key cannot be null.");
        }

        string path = Path.Combine(ClientDirectoryPath, Constants.PublicKeyFileName);
        using FileStream stream = new(path, FileMode.Append, FileAccess.Write);
        publicKey.Save(stream);
    }

    // TODO: test LoadPublicKey
    public PublicKey? LoadPublicKey()
    {
        string path = Path.Combine(ClientDirectoryPath, Constants.PublicKeyFileName);

        if (!File.Exists(path))
        {
            return null;
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        PublicKey publicKey = new();
        publicKey.Load(ClientConfig.EncryptionHelper.Context, stream);
        return publicKey;
    }

    // TODO: test SaveSecretKey
    public void SaveSecretKey(SecretKey secretKey)
    {
        if (secretKey == null)
        {
            throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null.");
        }

        string path = Path.Combine(ClientDirectoryPath, Constants.SecretKeyFileName);
        using FileStream stream = new(path, FileMode.Append, FileAccess.Write);
        secretKey.Save(stream);
    }

    // TODO: test LoadSecretKey
    public SecretKey? LoadSecretKey()
    {
        string path = Path.Combine(ClientDirectoryPath, Constants.SecretKeyFileName);

        if (!File.Exists(path))
        {
            return null;
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        SecretKey secretKey = new();
        secretKey.Load(ClientConfig.EncryptionHelper.Context, stream);
        return secretKey;
    }

    // TODO: test SaveRelinKeys
    public void SaveRelinKeys(Serializable<RelinKeys> relinKeys)
    {
        if (relinKeys == null)
        {
            throw new ArgumentNullException(nameof(relinKeys), "Relinearizaion keys cannot be null.");
        }

        string path = Path.Combine(ClientDirectoryPath, Constants.RelinKeysFileName);
        using FileStream stream = new(path, FileMode.Append, FileAccess.Write);
        relinKeys.Save(stream);
    }

    // TODO: test LoadRelinKeys
    public RelinKeys? LoadRelinKeys()
    {
        string path = Path.Combine(ClientDirectoryPath, Constants.RelinKeysFileName);

        if (!File.Exists(path))
        {
            return null;
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        RelinKeys relinKeys = new();
        relinKeys.Load(ClientConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }
}