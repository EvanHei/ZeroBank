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
    public DataAccessor()
    {
        Directory.CreateDirectory(Constants.ServerDirectoryPath);
    }

    // TODO: test
    public void SaveTransaction(Ciphertext transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
        }

        using FileStream stream = new(Constants.TransactionsFilePath, FileMode.Append, FileAccess.Write);
        transaction.Save(stream);
    }

    // TODO: test
    public List<Ciphertext> LoadTransactions()
    {
        List<Ciphertext> transactions = new();

        if (!File.Exists(Constants.TransactionsFilePath))
        {
            return transactions;
        }

        using FileStream stream = new(Constants.TransactionsFilePath, FileMode.Open, FileAccess.Read);

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

        using FileStream stream = new(Constants.RelinKeysFilePath, FileMode.Create, FileAccess.Write);
        relinKeys.Save(stream);
    }

    // TODO: test
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
}
