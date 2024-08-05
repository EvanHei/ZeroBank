using Microsoft.Research.SEAL;
using Microsoft.VisualBasic;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerLibrary;

public class JsonAccessor
{
    public JsonAccessor()
    {
        Directory.CreateDirectory(Constants.ServerDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
        Directory.CreateDirectory(Constants.PrivateKeysDirectoryPath);

        if (!File.Exists(Constants.UsersFilePath))
        {
            string json = JsonSerializer.Serialize(new List<User>());
            File.WriteAllText(Constants.UsersFilePath, json);
        }
    }

    public User LoadUser(UserCredentials userCredentials)
    {
        User user = LoadUsers().FirstOrDefault(u => u.Username.Equals(userCredentials.Username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(userCredentials.Password));
        return user;
    }

    public List<User> LoadUsers()
    {
        if (!File.Exists(Constants.UsersFilePath))
        {
            throw new FileNotFoundException($"File not found: {Constants.UsersFilePath}");
        }

        string json = File.ReadAllText(Constants.UsersFilePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json);
        return users;
    }

    public void CreateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentException("User cannot be null.");
        }

        List<User> users = LoadUsers();

        if (users.Any(u => u.Username == user.Username))
        {
            throw new ArgumentException("Username taken.");
        }

        users.Add(user);
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.UsersFilePath, json);
    }

    public RelinKeys LoadRelinKeysById(int id)
    {
        Account account = LoadAccountById(id);
        byte[] relinKeysBytes = account.SEALRelinKeys;
        using MemoryStream stream = new(relinKeysBytes);
        RelinKeys relinKeys = new();
        relinKeys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }

    public Account CreateAccount(Account account, byte[] serverSigningPrivateKey)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // save server signing private key
        string path = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Name + ".bin");
        File.WriteAllBytes(path, serverSigningPrivateKey);

        // save partial account
        SaveAccount(account);
        return account;
    }

    public void SaveAccount(Account account)
    {
        string json = account.SerializeToJson();

        // filename is format <Name>.json
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.WriteAllText(path, json);
    }

    public List<Account> LoadUserAccounts(int id)
    {
        List <Account> userAccounts = LoadAllAccounts().Where(a => a.UserId == id).ToList();
        return userAccounts;
    }

    public List<Account> LoadAllAccounts()
    {
        if (!Directory.Exists(Constants.AccountsDirectoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: { Constants.AccountsDirectoryPath}");
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
        Account account = LoadAllAccounts().Where(a => a.Id == id).FirstOrDefault() ?? throw new InvalidOperationException("Account not found.");
        return account;
    }

    public void DeleteAccountById(int id)
    {
        Account account = LoadAccountById(id);
        string accountPath = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.Delete(accountPath);

        // delete the key file
        string keyPath = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Name + ".bin");
        File.Delete(keyPath);
    }

    public List<Ciphertext> LoadTransactionsById(int id)
    {
        Account account = LoadAccountById(id);
        List<Ciphertext> transactions = new();

        foreach (Transaction transaction in account.Transactions)
        {
            try
            {
                using MemoryStream stream = new(transaction.Data);
                Ciphertext ciphertext = new();
                ciphertext.Load(ServerConfig.EncryptionHelper.Context, stream);
                transactions.Add(ciphertext);
            }
            catch (Exception ex)
            {
                // TODO: add logging
                continue;
            }        
        }
        return transactions;
    }

    public Transaction AddTransactionById(int id, Transaction transaction)
    {
        Account account = LoadAccountById(id);
        account.Transactions.Add(transaction);
        SaveAccount(account);
        return transaction;
    }

    public byte[] LoadSigningKeyById(int id)
    {
        Account? account = LoadAccountById(id);
        string path = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Name + ".bin");
        return File.ReadAllBytes(path);
    }
}
