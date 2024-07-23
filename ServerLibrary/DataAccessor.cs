using Microsoft.Research.SEAL;
using Microsoft.VisualBasic;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace ServerLibrary;

public class DataAccessor
{
    public DataAccessor()
    {
        Directory.CreateDirectory(Constants.ServerDirectoryPath);

        if (!File.Exists(Constants.AccountsFilePath))
        {
            string json = JsonSerializer.Serialize(new List<Ciphertext>());
            File.WriteAllText(Constants.AccountsFilePath, json);
        }
    }

    public void SaveRelinKeys(Stream stream)
    {
        if (stream.Length == 0)
        {
            return;
        }

        // try to deserialize successfully, then write to file
        try
        {
            RelinKeys relinKeys = new();
            relinKeys.Load(ServerConfig.EncryptionHelper.Context, stream);
            stream.Seek(0, SeekOrigin.Begin);
            using FileStream fileStream = new(Constants.RelinKeysFilePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);

        }
        catch (Exception ex)
        {
            // TODO: log failure
        }    
    }

    public RelinKeys? LoadRelinKeys()
    {
        if (!File.Exists(Constants.RelinKeysFilePath))
        {
            return null;
        }

        using FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Open, FileAccess.Read);
        RelinKeys keys = new();
        keys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return keys;
    }

    public void AddAccount(AccountModel account)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        List<AccountModel> accounts = LoadAccounts();
        account.Id = accounts.Count != 0 ? accounts.Max(a => a.Id) + 1 : 1;
        accounts.Add(account);
        SaveAccounts(accounts);
    }

    private void SaveAccounts(List<AccountModel> accounts)
    {
        string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.AccountsFilePath, json);
    }

    public List<AccountModel> LoadAccounts()
    {
        if (!File.Exists(Constants.AccountsFilePath))
        {
            throw new FileNotFoundException($"File not found: { Constants.AccountsFilePath }");
        }

        string json = File.ReadAllText(Constants.AccountsFilePath);
        List<AccountModel> accounts = JsonSerializer.Deserialize<List<AccountModel>>(json);
        return accounts;
    }

    public AccountModel? LoadAccount(int accountId)
    {
        AccountModel? account = LoadAccounts().Where(a => a.Id == accountId).FirstOrDefault();
        return account;
    }

    public void DeleteAccount(int accountId)
    {
        List<AccountModel> accounts = LoadAccounts();
        AccountModel? accountToRemove = accounts.FirstOrDefault(a => a.Id == accountId) ?? throw new InvalidOperationException("Account not found.");
        accounts.Remove(accountToRemove);
        SaveAccounts(accounts);
    }

    public List<Ciphertext> LoadTransactions(int accountId)
    {
        AccountModel? account = LoadAccounts().FirstOrDefault(a => a.Id == accountId) ?? throw new InvalidOperationException("Account not found.");
        List<Ciphertext> transactions = account.GetTransactions(ServerConfig.EncryptionHelper.Context);
        return transactions;
    }

    public void AddTransaction(Stream stream, int accountId)
    {
        List<AccountModel> accounts = LoadAccounts();
        AccountModel? account = accounts.FirstOrDefault(a => a.Id == accountId) ?? throw new InvalidOperationException("Account not found.");
        account.AddTransactions(stream, ServerConfig.EncryptionHelper.Context);
        SaveAccounts(accounts);
    }
}
