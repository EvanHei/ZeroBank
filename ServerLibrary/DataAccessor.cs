using Microsoft.Research.SEAL;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ServerLibrary;

public class DataAccessor
{
    private string ServerDirectoryPath { get; set; }

    public DataAccessor()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDirectoryPath = Path.Combine(appDataPath, Constants.AppDirectoryName);
        ServerDirectoryPath = Path.Combine(appDirectoryPath, Constants.ServerDirectoryName);
        Directory.CreateDirectory(ServerDirectoryPath);
    }

    // TODO: test
    public void SaveTransaction(Ciphertext transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
        }

        string path = Path.Combine(ServerDirectoryPath, Constants.TransactionsFileName);
        using FileStream stream = new(path, FileMode.Append, FileAccess.Write);
        transaction.Save(stream);
    }

    // TODO: test
    public List<Ciphertext> LoadTransactions()
    {
        List<Ciphertext> transactions = new();
        string path = Path.Combine(ServerDirectoryPath, Constants.TransactionsFileName);

        if (!File.Exists(path))
        {
            return transactions;
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);

        while (stream.Position < stream.Length)
        {
            Ciphertext transaction = new();
            transaction.Load(ServerConfig.EncryptionHelper.Context, stream);
            transactions.Add(transaction);
        }

        return transactions;
    }

    // TODO: test
    public void SaveRelinKeys(RelinKeys relinKeys)
    {
        if (relinKeys == null)
        {
            throw new ArgumentNullException(nameof(relinKeys), "Relinearization keys cannot be null.");
        }

        string path = Path.Combine(ServerDirectoryPath, Constants.RelinKeysFileName);
        using FileStream stream = new(path, FileMode.Create, FileAccess.Write);
        relinKeys.Save(stream);
    }

    // TODO: test
    public RelinKeys? LoadRelinKeys()
    {
        string path = Path.Combine(ServerDirectoryPath, Constants.RelinKeysFileName);

        if (!File.Exists(path))
        {
            return null;
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        RelinKeys keys = new();
        keys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return keys;
    }
}
