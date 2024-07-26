using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientLibrary;

public class JsonAccessor
{
    public JsonAccessor()
    {
        Directory.CreateDirectory(Constants.ClientDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
    }

    // TODO: write to account, not files
    public void SavePublicKey(PublicKey publicKey)
    {
        if (publicKey == null)
        {
            throw new ArgumentNullException(nameof(publicKey), "Public key cannot be null.");
        }

        using FileStream stream = new(Constants.PublicKeyFilePath, FileMode.Append, FileAccess.Write);
        publicKey.Save(stream);
    }

    // TODO: write to account, not files
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

    // TODO: write to account, not files
    public void SaveSecretKey(SecretKey secretKey)
    {
        if (secretKey == null)
        {
            throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null.");
        }

        using FileStream stream = new(Constants.SecretKeyFilePath, FileMode.Append, FileAccess.Write);
        secretKey.Save(stream);
    }

    // TODO: write to account, not files
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

    public void CreateAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // TODO: verify server signature

        SaveAccount(account);
    }

    private void SaveAccount(Account account)
    {
        string json = account.SerializeToJson();

        // filename is format <accountId>.json
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.WriteAllText(path, json);
    }

    public List<Account> LoadAccounts()
    {
        if (!Directory.Exists(Constants.AccountsDirectoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {Constants.AccountsDirectoryPath}");
        }

        List<Account> accounts = new();

        string[] accountFiles = Directory.GetFiles(Constants.AccountsDirectoryPath);
        foreach (string accountFile in accountFiles)
        {
            string json = File.ReadAllText(accountFile);
            Account account = JsonSerializer.Deserialize<Account>(json);
            accounts.Add(account);
        }
        return accounts;
    }

    private Account? LoadAccountById(int id)
    {
        Account? account = LoadAccounts().Where(a => a.Id == id).FirstOrDefault() ?? throw new InvalidOperationException("Account not found.");
        account?.EnsureValid();
        return account;
    }

    public void DeleteAccountById(int id)
    {
        Account? account = LoadAccountById(id);
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.Delete(path);
    }

    public void AddTransactionById(Transaction transaction, int id)
    {
        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Data);
        using Ciphertext ciphertext = new();
        ciphertext.Load(ClientConfig.EncryptionHelper.Context, stream);

        Account? account = LoadAccountById(id);

        // TODO: verify server signature and then sign

        account.Transactions.Add(transaction);
        SaveAccount(account);
    }
}