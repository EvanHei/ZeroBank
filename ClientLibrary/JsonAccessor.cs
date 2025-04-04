using SharedLibrary.Models;
using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System.Text.Json;

namespace SharedLibrary;

public class JsonAccessor
{
    public JsonAccessor()
    {
        Directory.CreateDirectory(Constants.ClientDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
    }

    public PublicKey LoadPublicKeyById(int id, SEALContext context)
    {
        if (context == null)
        {
            return null;
        }

        byte[] publicKeyBytes = LoadAccount(id).SEALPublicKey;
        using MemoryStream stream = new(publicKeyBytes);
        PublicKey publicKey = new();
        publicKey.Load(context, stream);
        return publicKey;
    }

    public SecretKey LoadSecretKey(int id, SEALContext context, string password)
    {
        if (context == null)
        {
            return null;
        }

        byte[] encryptedSecretKeyBytes = LoadAccount(id).SEALSecretKeyEncrypted;

        // decrypt
        Pbkdf2KeyDeriver keyDeriver = new();
        AesEncryptor aes = new();
        byte[] key = keyDeriver.DeriveKey(password, new byte[0]);
        byte[] secretKeyBytes = aes.Decrypt(encryptedSecretKeyBytes, key);

        using MemoryStream stream = new(secretKeyBytes);
        SecretKey secretKey = new();
        secretKey.Load(context, stream);
        return secretKey;
    }

    public EncryptionParameters LoadParms(int id)
    {
        byte[] parmsBytes = LoadAccount(id).Parms;
        using MemoryStream stream = new(parmsBytes);
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

        SaveAccount(account);
    }

    public void SaveAccount(Account account)
    {
        string json = account.SerializeToJson();
        string path = Path.Combine(Constants.AccountsDirectoryPath, account.Id + ".json");
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

    public Account LoadAccount(int accountId)
    {
        Account account = LoadAccounts().Where(a => a.Id == accountId).FirstOrDefault() ?? throw new InvalidOperationException($"Account with ID {accountId} not found.");
        return account;
    }

    public void AddTransaction(CiphertextTransaction transaction, SEALContext context)
    {
        Account account = LoadAccount(transaction.AccountId);
        account.Transactions.Add(transaction);
        SaveAccount(account);
    }

    public List<CiphertextTransaction> LoadAllCiphertextTransactions()
    {
        List<CiphertextTransaction> ciphertextTransactions = new();
        List<Account> accounts = LoadAccounts();

        foreach (Account account in accounts)
        {
            foreach (CiphertextTransaction ciphertextTransaction in account.Transactions)
            {
                ciphertextTransactions.Add(ciphertextTransaction);
            }
        }

        return ciphertextTransactions;
    }
}