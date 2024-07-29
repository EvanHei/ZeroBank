using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary;

public static class ServerConfig
{
    public static JsonAccessor DataAccessor { get; set; } = new JsonAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();

    public static void CreateAccount(Account account)
    {
        // ensure the account name is not taken
        List<Account> accounts = DataAccessor.LoadAccounts();
        if (accounts.Any(a => a.Name == account.Name))
        {
            throw new InvalidOperationException($"Account with name {account.Name} already exists.");
        }

        // get a new account ID
        account.Id = accounts.Count != 0 ? accounts.Max(a => a.Id) + 1 : 1;

        // get a new server signing key pair and sign
        RsaSigner rsa = new();
        (byte[] serverSigningPublicKey, byte[] serverSigningPrivateKey) = rsa.GenerateKeyPair();
        account.ServerSigningPublicKey = serverSigningPublicKey;
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, account.SerializeMetadataToBytes());
        account.ServerDigSig = serverDigSig;

        // save to server
        DataAccessor.CreateAccount(account, serverSigningPrivateKey);
    }

    public static void AddTransactionById(int id, Transaction transaction)
    {
        Account account = DataAccessor.LoadAccountById(id);

        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Data);
        using Ciphertext ciphertext = new();
        ciphertext.Load(EncryptionHelper.Context, stream);

        // load server signing key
        byte[] serverSigningPrivateKey = DataAccessor.LoadSigningKeyById(id);

        // sign
        RsaSigner rsa = new();
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, transaction.Data);
        transaction.ServerDigSig = serverDigSig;

        // verify signatures
        account.EnsureValid();

        DataAccessor.AddTransactionById(id, transaction);
    }

    public static MemoryStream GetBalanceStreamById(int id)
    {
        // verify signatures
        DataAccessor.LoadAccountById(id).EnsureValid();

        List<Ciphertext> transactions = DataAccessor.LoadTransactionsById(id);
        using RelinKeys? relinKeys = DataAccessor.LoadRelinKeysById(id);
        using Ciphertext? balance = EncryptionHelper.GetBalance(transactions, relinKeys) ?? throw new NullReferenceException("There are no transactions.");

        MemoryStream stream = new();
        balance.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
