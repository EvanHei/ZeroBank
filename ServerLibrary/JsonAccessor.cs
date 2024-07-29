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
    }

    public RelinKeys? LoadRelinKeysById(int id)
    {
        Account? account = LoadAccountById(id);
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

    public List<Account> LoadAccounts()
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

    public Account? LoadAccountById(int id)
    {
        Account? account = LoadAccounts().Where(a => a.Id == id).FirstOrDefault() ?? throw new InvalidOperationException("Account not found.");
        return account;
    }

    public void DeleteAccountById(int id)
    {
        Account? account = LoadAccountById(id);
        string accountPath = Path.Combine(Constants.AccountsDirectoryPath, $"{account.Name}.json");
        File.Delete(accountPath);

        // delete the key file
        string keyPath = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Name + ".bin");
        File.Delete(keyPath);
    }

    public List<Ciphertext> LoadTransactionsById(int id)
    {
        Account? account = LoadAccountById(id);
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
