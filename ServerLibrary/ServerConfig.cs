using Microsoft.Research.SEAL;
using SharedLibrary;
using SharedLibrary.Models;
using System.Data.Common;
using System.Transactions;

namespace ServerLibrary;

public static class ServerConfig
{
    public static JsonAccessor DataAccessor { get; set; } = new JsonAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();

    public static void CreateUser(Credentials userCredentials)
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

        // hash the password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userCredentials.Password);

        User newUser = new(userCredentials.Username, passwordHash);

        // set user ID
        List<User> users = DataAccessor.LoadUsers();
        newUser.Id = users.Count != 0 ? users.Max(u => u.Id) + 1 : 1;

        DataAccessor.CreateUser(newUser);
    }

    public static void CreateAdmin(Credentials adminCredentials)
    {
        if (string.IsNullOrEmpty(adminCredentials.Username) || string.IsNullOrEmpty(adminCredentials.Password))
        {
            throw new ArgumentException("Username and password must not be empty.");
        }

        // check if the admin already exists
        User admin = DataAccessor.LoadAdmin(adminCredentials);
        if (admin != null)
        {
            throw new InvalidOperationException("A user with the provided credentials already exists.");
        }

        // hash the password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(adminCredentials.Password);

        User newAdmin = new(adminCredentials.Username, passwordHash);

        // set user ID
        List<User> admins = DataAccessor.LoadAdmins();
        newAdmin.Id = admins.Count != 0 ? admins.Max(u => u.Id) + 1 : 1;

        DataAccessor.CreateAdmin(newAdmin);
    }

    public static User LoadUser(Credentials userCredentials)
    {
        if (string.IsNullOrEmpty(userCredentials.Username) || string.IsNullOrEmpty(userCredentials.Password))
        {
            throw new ArgumentException("Username and password must not be empty.");
        }

        // get user from database
        User user = DataAccessor.LoadUser(userCredentials);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        return user;
    }

    public static User LoadAdmin(Credentials adminCredentials)
    {
        if (string.IsNullOrEmpty(adminCredentials.Username) || string.IsNullOrEmpty(adminCredentials.Password))
        {
            throw new ArgumentException("Username and password must not be empty.");
        }

        // get admin from database
        User admin = DataAccessor.LoadAdmin(adminCredentials);
        if (admin == null)
        {
            throw new InvalidOperationException("Admin not found.");
        }

        return admin;
    }

    public static void CreatePartialAccount(Account account, int userId)
    {
        // ensure the account name is not taken
        List<Account> accounts = DataAccessor.LoadAllAccounts();

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
        AuthorizeAccountAccess(account.Id, userId);
        account.EnsureValid();
        DataAccessor.SaveAccount(account);
    }

    public static void AddTransaction(int userId, CiphertextTransaction transaction)
    {
        Account account = DataAccessor.LoadAccount(transaction.AccountId);

        // verify the user owns the account
        AuthorizeAccountAccess(transaction.AccountId, userId);

        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Ciphertext);
        using Ciphertext ciphertext = new();
        ciphertext.Load(EncryptionHelper.Context, stream);

        // load server signing key
        byte[] serverSigningPrivateKey = DataAccessor.LoadSigningKey(transaction.AccountId);

        // sign transaction
        RsaSigner rsa = new();
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, transaction.SerializeMetadataToBytes());
        transaction.ServerDigSig = serverDigSig;

        // verify signatures
        account.EnsureValid();

        // save to server
        DataAccessor.AddTransaction(transaction);
    }

    private static void AuthorizeAccountAccess(int accountId, int userId)
    {
        Account account = DataAccessor.LoadAccount(accountId);
        if (account.UserId != userId)
        {
            throw new UnauthorizedAccessException("The user does not have permission to access this account.");
        }
    }

    public static Account CloseAccount(Account account, int userId, byte[] key)
    {
        // verify the user owns the account
        AuthorizeAccountAccess(account.Id, userId);

        // load server signing key
        byte[] serverSigningPrivateKey = DataAccessor.LoadSigningKey(account.Id);

        // sign metadata
        RsaSigner rsa = new();
        byte[] serverDigSig = rsa.Sign(serverSigningPrivateKey, account.SerializeMetadataToBytes());
        account.ServerDigSig = serverDigSig;

        // verify signatures
        account.EnsureValid();

        // save to server
        DataAccessor.CloseAccount(account, key);

        return account;
    }
}
