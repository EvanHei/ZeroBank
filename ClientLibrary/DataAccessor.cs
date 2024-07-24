using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace ClientLibrary;

public class DataAccessor
{
    public DataAccessor()
    {
        Directory.CreateDirectory(Constants.ClientDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
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

        using FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Open, FileAccess.Write);
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

    public Stream? LoadRelinKeysStream()
    {
        if (!File.Exists(Constants.RelinKeysFilePath))
        {
            return null;
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            return null;
        }

        FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Open, FileAccess.Read);
        return stream;
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

    public void CreateAccount(GenesisBlockData genesisBlockData)
    {
        if (genesisBlockData == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // get a new accountId
        List<Blockchain> accounts = LoadAccounts();
        genesisBlockData.AccountId = accounts.Count != 0 ? accounts.Max(a => a.GetData<GenesisBlockData>(0).AccountId) + 1 : 1;

        // TODO: verify server signature

        // create and save new account blockchain
        Blockchain account = new();
        account.AddBlock(genesisBlockData);
        SaveAccount(account);
    }

    private void SaveAccount(Blockchain account)
    {
        string json = account.SerializeToJson();

        // filename is format <accountId>.json
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{account.GetData<GenesisBlockData>(0).AccountId}.json");
        File.WriteAllText(path, json);
    }

    public List<Blockchain> LoadAccounts()
    {
        if (!Directory.Exists(Constants.AccountsDirectoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {Constants.AccountsDirectoryPath}");
        }

        List<Blockchain> accounts = new();

        string[] accountFiles = Directory.GetFiles(Constants.AccountsDirectoryPath);
        foreach (string accountFile in accountFiles)
        {
            string json = File.ReadAllText(accountFile);
            Blockchain account = JsonSerializer.Deserialize<Blockchain>(json);
            accounts.Add(account);
        }

        return accounts;
    }

    public Blockchain? LoadAccountById(int accountId)
    {
        Blockchain? account = LoadAccounts().Where(a => a.GetData<GenesisBlockData>(0).AccountId == accountId).FirstOrDefault();
        return account;
    }

    public void DeleteAccountById(int accountId)
    {
        List<Blockchain> accounts = LoadAccounts();
        Blockchain? accountToRemove = accounts.FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{accountToRemove.GetData<GenesisBlockData>(0).AccountId}.json");
        File.Delete(path);
    }

    public void AddTransactionById(TransactionBlockData transactionBlockData, int accountId)
    {
        // verify data will generate a ciphertext
        using MemoryStream stream = new(transactionBlockData.Transaction);
        Ciphertext transaction = new();
        transaction.Load(ClientConfig.EncryptionHelper.Context, stream);

        Blockchain? account = LoadAccounts().FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");

        // TODO: verify server signature

        account.AddBlock(transactionBlockData);
        SaveAccount(account);
    }
}