using ClientLibrary.Models;
using Microsoft.Research.SEAL;
using SharedLibrary;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public static class ClientConfig
{
    public static JsonAccessor DataAccessor { get; set; } = new JsonAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task CreateAccount(string name, AccountType type, string password)
    {
        // get encryption parameters from server
        using MemoryStream parmsStream = new();
        using EncryptionParameters parms = await ApiAccessor.GetEncryptionParameters();
        parms.Save(parmsStream);

        // generate keys
        (PublicKey SEALPublicKey,
         SecretKey SEALSecretKey,
         Serializable<RelinKeys> SEALRelinKeys,
         byte[] clientSigningPublicKey,
         byte[] clientSigningPrivateKey) = EncryptionHelper.GenerateKeys(parms);

        // save SEAL keys to streams
        using MemoryStream SEALPublicKeyStream = new();
        using MemoryStream SEALSecretKeyStream = new();
        using MemoryStream SEALRelinKeysStream = new();
        SEALPublicKey.Save(SEALPublicKeyStream);
        SEALSecretKey.Save(SEALSecretKeyStream);
        SEALRelinKeys.Save(SEALRelinKeysStream);

        // encrypt the SEAL secret key
        Pbkdf2KeyDeriver keyDeriver = new();
        AesEncryptor aes = new();
        byte[] key = keyDeriver.DeriveKey(password, new byte[0]);
        byte[] SEALSecretKeyBytes = SEALSecretKeyStream.ToArray();
        byte[] SEALSecretKeyBytesEncrypted = aes.Encrypt(SEALSecretKeyBytes, key);

        // encrypt the client signing private key
        byte[] clientSigningPrivateKeyEncrypted = aes.Encrypt(clientSigningPrivateKey, key);

        // create account
        Account account = new(name,
                              type,
                              DateTime.Now,
                              parmsStream.ToArray(),
                              SEALPublicKeyStream.ToArray(),
                              SEALSecretKeyBytesEncrypted,
                              SEALRelinKeysStream.ToArray(),
                              clientSigningPublicKey,
                              clientSigningPrivateKeyEncrypted);

        // post to server to get an ID and the server's public signing key
        Account returnedAccount = await ApiAccessor.PostPartialAccount(account);

        // sign
        RsaSigner rsa = new();
        byte[] clientDigSig = rsa.Sign(clientSigningPrivateKey, returnedAccount.SerializeMetadataToBytes());
        returnedAccount.ClientDigSig = clientDigSig;

        // send to server again
        await ApiAccessor.PostFullAccount(returnedAccount);

        // verify digital signatures
        returnedAccount.EnsureValid();

        // save to client
        DataAccessor.CreateAccount(returnedAccount);
    }

    public static async Task AddTransaction(int accountId, long amount, string password)
    {
        Account account = DataAccessor.LoadAccount(accountId);

        // ensure the amount is within the cryptographic range
        using EncryptionParameters parms = DataAccessor.LoadParms(accountId);
        int maxValue = (int)((parms.PlainModulus.Value - 1) / 2);
        if (Math.Abs(amount) > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, $"The amount must be in range of ±{maxValue}.");
        }

        // get encryption data
        using SEALContext context = new(parms);
        using PublicKey publicKey = DataAccessor.LoadPublicKeyById(accountId, context);
        using SecretKey secretKey = DataAccessor.LoadSecretKey(accountId, context, password);

        // decrypt client signing encrypted key
        Pbkdf2KeyDeriver keyDeriver = new();
        AesEncryptor aes = new();
        byte[] key = keyDeriver.DeriveKey(password);
        byte[] clientSigningPrivateKey = aes.Decrypt(account.ClientSigningPrivateKeyEncrypted, key);

        // generate ciphertext and create transaction
        using MemoryStream ciphertextStream = new();
        EncryptionHelper.Encrypt(amount, context, publicKey, secretKey).Save(ciphertextStream);
        CiphertextTransaction transaction = new(ciphertextStream.ToArray(), accountId);

        // sign
        RsaSigner rsa = new();
        byte[] clientDigSig = rsa.Sign(clientSigningPrivateKey, transaction.SerializeMetadataToBytes());
        transaction.ClientDigSig = clientDigSig;

        // post to server
        CiphertextTransaction returnedTransaction = await ApiAccessor.PostTransaction(transaction);

        // verify signatures
        account.EnsureValid();

        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Ciphertext);
        using Ciphertext ciphertext = new();
        ciphertext.Load(context, stream);

        // save to client
        DataAccessor.AddTransaction(returnedTransaction, context);
    }

    public static async Task<long> GetBalance(int accountId, string password)
    {
        using EncryptionParameters parms = DataAccessor.LoadParms(accountId);
        using SEALContext context = new(parms);
        using SecretKey secretKey = DataAccessor.LoadSecretKey(accountId, context, password);

        using Stream stream = await ApiAccessor.GetBalanceStream(accountId);

        using MemoryStream memStream = new();
        stream.CopyTo(memStream);
        if (memStream.Length == 0)
        {
            return 0L;
        }
        memStream.Seek(0, SeekOrigin.Begin);

        using Ciphertext ciphertext = new();
        ciphertext.Load(context, memStream);

        long balance = EncryptionHelper.Decrypt(ciphertext, context, secretKey);
        return balance;
    }

    public static async Task DeleteAccount(int accountId)
    {
        await ApiAccessor.DeleteAccount(accountId);
        DataAccessor.DeleteAccount(accountId);
    }

    public static async Task<List<PlaintextTransaction>> GetPlaintextTransactions(int accountId, string password)
    {
        List<PlaintextTransaction> plaintextTransactions = new();

        List<Account> accounts = await ApiAccessor.GetAccounts();
        Account account = accounts.FirstOrDefault(a => a.Id == accountId);

        using EncryptionParameters parms = DataAccessor.LoadParms(accountId);
        using SEALContext context = new(parms);
        using SecretKey secretKey = DataAccessor.LoadSecretKey(accountId, context, password);

        foreach (CiphertextTransaction ciphertextTransaction in account.Transactions)
        {
            using MemoryStream memStream = new(ciphertextTransaction.Ciphertext);
            Ciphertext ciphertext = new();
            ciphertext.Load(context, memStream);

            PlaintextTransaction plaintextTransaction = new();
            plaintextTransaction.Amount = EncryptionHelper.Decrypt(ciphertext, context, secretKey);
            plaintextTransaction.AccountId = ciphertextTransaction.AccountId;
            plaintextTransaction.Timestamp = ciphertextTransaction.Timestamp;

            plaintextTransactions.Add(plaintextTransaction);
        }

        return plaintextTransactions;
    }

    public static int GetMaxAmount(int accountId)
    {
        using EncryptionParameters parms = DataAccessor.LoadParms(accountId);
        int max = (int)((parms.PlainModulus.Value - 1) / 2);
        return max;
    }
}
