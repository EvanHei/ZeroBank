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

    public void SaveTransaction(Stream stream)
    {
        if (stream.Length == 0)
        {
            return;
        }

        // try to deserialize successfully, then write to file
        try
        {
            Ciphertext transaction = new();
            transaction.Load(ServerConfig.EncryptionHelper.Context, stream);
            stream.Seek(0, SeekOrigin.Begin);
            using FileStream fileStream = new(Constants.TransactionsFilePath, FileMode.Append, FileAccess.Write);
            stream.CopyTo(fileStream);
        }
        catch (Exception)
        {
            // TODO: log failure
        }
    }

    public List<Ciphertext>? LoadTransactions()
    {
        List<Ciphertext> transactions = new();

        if (!File.Exists(Constants.TransactionsFilePath))
        {
            return null;
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
}
