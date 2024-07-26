using Microsoft.Research.SEAL;
using Microsoft.VisualBasic;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SharedLibrary.Blockchain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerLibrary;

public class JsonAccessor
{
    public JsonAccessor()
    {
        Directory.CreateDirectory(Constants.ServerDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
    }

    public RelinKeys? LoadRelinKeysById(int id)
    {
        Account? account = LoadAccountById(id);
        byte[] relinKeysBytes = account.RelinKeys;
        using MemoryStream stream = new(relinKeysBytes);
        RelinKeys relinKeys = new();
        relinKeys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }

    public Account CreateAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // get a new account ID
        List<Account> accounts = LoadAccounts();
        account.Id = accounts.Count != 0 ? accounts.Max(a => a.Id) + 1 : 1;

        // TODO: verify client signature and then sign

        // save new account
        SaveAccount(account);
        return account;
    }

    private void SaveAccount(Account account)
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

    public List<Ciphertext> LoadTransactionsById(int id)
    {
        Account? account = LoadAccountById(id);
        List<Ciphertext> transactions = new();
        account?.EnsureValid();

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

    public Transaction AddTransactionById(Transaction transaction, int id)
    {
        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Data);
        using Ciphertext ciphertext = new();
        ciphertext.Load(ServerConfig.EncryptionHelper.Context, stream);

        Account? account = LoadAccountById(id);

        // TODO: verify client signature and then sign

        account.Transactions.Add(transaction);
        SaveAccount(account);
        return transaction;
    }
}
