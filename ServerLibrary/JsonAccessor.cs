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
using System.Transactions;
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

    public RelinKeys? LoadRelinKeysById(int accountId)
    {
        Blockchain? account = LoadAccounts().FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");
        byte[] relinKeysBytes = account.GetData<GenesisBlockData>(0).RelinKeys;
        MemoryStream stream = new(relinKeysBytes);
        RelinKeys relinKeys = new();
        relinKeys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }

    public GenesisBlockData CreateAccount(GenesisBlockData genesisBlockData)
    {
        if (genesisBlockData == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // get a new account ID
        List<Blockchain> accounts = LoadAccounts();
        genesisBlockData.AccountId = accounts.Count != 0 ? accounts.Max(a => a.GetData<GenesisBlockData>(0).AccountId) + 1 : 1;

        // TODO: verify client signature and then sign

        // create and save new account blockchain
        Blockchain account = new();
        account.AddBlock(genesisBlockData);
        SaveAccount(account);
        return genesisBlockData;
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
            throw new DirectoryNotFoundException($"Directory not found: { Constants.AccountsDirectoryPath}");
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
        account?.EnsureValid();
        return account;
    }

    public void DeleteAccountById(int accountId)
    {
        List<Blockchain> accounts = LoadAccounts();
        Blockchain? accountToRemove = accounts.FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");
        string path = Path.Combine(Constants.AccountsDirectoryPath, $"{accountToRemove.GetData<GenesisBlockData>(0).AccountId}.json");
        File.Delete(path);
    }

    public List<Ciphertext> LoadTransactionsById(int accountId)
    {
        Blockchain? account = LoadAccounts().FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");
        List<Ciphertext> transactions = new();
        account?.EnsureValid();

        for (int i = 1; i < account.Chain.Count; i++)
        {
            try
            {
                byte[] transactionBytes = account.GetData<TransactionBlockData>(i).Transaction;
                using MemoryStream stream = new(transactionBytes);
                Ciphertext transaction = new();
                transaction.Load(ServerConfig.EncryptionHelper.Context, stream);
                transactions.Add(transaction);

            }
            catch (Exception ex)
            {
                // TODO: add logging
                continue;
            }        
        }
        return transactions;
    }

    public TransactionBlockData AddTransactionById(TransactionBlockData transactionBlockData, int accountId)
    {
        // verify data will generate a ciphertext
        using MemoryStream stream = new(transactionBlockData.Transaction);
        Ciphertext transaction = new();
        transaction.Load(ServerConfig.EncryptionHelper.Context, stream);

        Blockchain? account = LoadAccounts().FirstOrDefault(a => a.GetData<GenesisBlockData>(0).AccountId == accountId) ?? throw new InvalidOperationException("Account not found.");

        // TODO: verify client signature and then sign

        account.AddBlock(transactionBlockData);
        SaveAccount(account);
        return transactionBlockData;
    }
}
