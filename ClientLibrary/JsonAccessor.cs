using Microsoft.Research.SEAL;
using SharedLibrary;
using System.Text.Json;

namespace ClientLibrary;

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

        byte[] publicKeyBytes = LoadAccountById(id).SEALPublicKey;
        using MemoryStream stream = new(publicKeyBytes);
        PublicKey publicKey = new();
        publicKey.Load(context, stream);
        return publicKey;
    }

    public SecretKey LoadSecretKeyById(int id, SEALContext context, string password)
    {
        if (context == null)
        {
            return null;
        }

        byte[] encryptedSecretKeyBytes = LoadAccountById(id).SEALSecretKeyEncrypted;

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

    public EncryptionParameters LoadParmsById(int id)
    {
        byte[] parmsBytes = LoadAccountById(id).Parms;
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

        // filename is format <Name>.json
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

    public Account LoadAccountById(int id)
    {
        Account account = LoadAccounts().Where(a => a.Id == id).FirstOrDefault() ?? throw new InvalidOperationException($"Account with ID {id} not found.");
        return account;
    }

    public void DeleteAccountById(int id)
    {
        Account account = LoadAccountById(id);
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.Delete(path);
    }

    public void AddTransactionById(int id, Transaction transaction, SEALContext context)
    {
        Account account = LoadAccountById(id);
        account.Transactions.Add(transaction);
        SaveAccount(account);
    }
}