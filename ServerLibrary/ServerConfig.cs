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

    public static void CreateUser(UserCredentials userCredentials)
    {
        if (string.IsNullOrEmpty(userCredentials.Username) || string.IsNullOrEmpty(userCredentials.Password))
        {
            throw new ArgumentException("Username and password must not be empty.");
        }

        // check if the user already exists
        User user = DataAccessor.LoadUser(userCredentials);
        if (user != null)
        {
            throw new InvalidOperationException("A user with the provided credentials already exists.");
        }

        // TODO: hash the password with BCrypt
        User newUser = new(userCredentials.Username, userCredentials.Password);

        // set user ID
        List<User> users = DataAccessor.LoadUsers();
        newUser.Id = users.Count != 0 ? users.Max(u => u.Id) + 1 : 1;

        DataAccessor.CreateUser(newUser);
    }

    public static void CreatePartialAccount(Account account, int userId)
    {
        // ensure the account name is not taken
        List<Account> accounts = DataAccessor.LoadAllAccounts();
        if (accounts.Any(a => a.Name == account.Name))
        {
            throw new InvalidOperationException($"Account with name {account.Name} already exists.");
        }

        // set account ID
        account.Id = accounts.Count != 0 ? accounts.Max(a => a.Id) + 1 : 1;

        // set account UserId
        account.UserId = userId;

        // get a new server signing key pair and sign
        RsaSigner rsa = new();
        (byte[] serverSigningPublicKey, byte[] serverSigningPrivateKey) = rsa.GenerateKeyPair();
        account.ServerSigningPublicKey = serverSigningPublicKey;
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, account.SerializeMetadataToBytes());
        account.ServerDigSig = serverDigSig;

        // save to server
        DataAccessor.CreateAccount(account, serverSigningPrivateKey);
    }

    public static void CreateFullAccount(int userId, Account account)
    {
        // verify the user owns the account
        AuthorizeUser(account.Id, userId);
        account.EnsureValid();
        DataAccessor.SaveAccount(account);
    }

    public static void AddTransactionById(int accountId, int userId, Transaction transaction)
    {
        Account account = DataAccessor.LoadAccountById(accountId);

        // verify the user owns the account
        AuthorizeUser(accountId, userId);

        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Data);
        using Ciphertext ciphertext = new();
        ciphertext.Load(EncryptionHelper.Context, stream);

        // load server signing key
        byte[] serverSigningPrivateKey = DataAccessor.LoadSigningKeyById(accountId);

        // sign
        RsaSigner rsa = new();
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, transaction.Data);
        transaction.ServerDigSig = serverDigSig;

        // verify signatures
        account.EnsureValid();

        DataAccessor.AddTransactionById(accountId, transaction);
    }

    public static MemoryStream GetBalanceStreamById(int accountId, int userId)
    {
        // verify signatures
        DataAccessor.LoadAccountById(accountId).EnsureValid();

        // verify the user owns the account
        AuthorizeUser(accountId, userId);

        List<Ciphertext> transactions = DataAccessor.LoadTransactionsById(accountId);
        using RelinKeys relinKeys = DataAccessor.LoadRelinKeysById(accountId);
        using Ciphertext balance = EncryptionHelper.GetBalance(transactions, relinKeys) ?? throw new NullReferenceException("There are no transactions.");

        MemoryStream stream = new();
        balance.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private static void AuthorizeUser(int accountId, int userId)
    {
        Account account = DataAccessor.LoadAccountById(accountId);
        if (account.UserId != userId)
        {
            throw new UnauthorizedAccessException("The user does not have permission to access this account.");
        }
    }

    public static void DeleteAccount(int accountId, int userId)
    {
        // verify the user owns the account
        AuthorizeUser(accountId, userId);
        DataAccessor.DeleteAccountById(accountId);
    }
}
