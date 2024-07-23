using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SharedLibrary;

public class AccountModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AccountType Type { get; set; }
    public byte[] Transactions { get; set; }

    public AccountModel(string name, AccountType type)
    {
        Name = name;
        Type = type;
    }

    public List<Ciphertext> GetTransactions(SEALContext context)
    {
        List<Ciphertext> transactions = new();

        if (Transactions == null)
        {
            return transactions;
        }

        // load the current transactions, if any
        using MemoryStream stream = new();
        if (Transactions != null)
        {
            stream.Write(Transactions, 0, Transactions.Length);
        }
        stream.Seek(0, SeekOrigin.Begin);

        // test to see if the stream contains valid ciphertexts and then add
        while (stream.Position < stream.Length)
        {
            Ciphertext transaction = new();
            transaction.Load(context, stream);
            transactions.Add(transaction);
        }

        return transactions;
    }

    public void AddTransactions(Stream stream, SEALContext context)
    {
        stream.Seek(0, SeekOrigin.Begin);

        // test to see if the stream contains valid ciphertexts
        while (stream.Position < stream.Length)
        {
            using Ciphertext transaction = new();
            transaction.Load(context, stream);
        }
        stream.Seek(0, SeekOrigin.Begin);

        // load the current transactions, if any
        using MemoryStream memStream = new();
        if (Transactions != null)
        {
            memStream.Write(Transactions, 0, Transactions.Length);
        }

        // save the new transactions to the end of the stream
        stream.CopyTo(memStream);
        Transactions = memStream.ToArray();
    }
}
