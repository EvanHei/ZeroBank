using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
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

    public static async Task AddTransactionById(int id, long amount, string password)
    {
        Account account = DataAccessor.LoadAccountById(id);

        // get encryption data
        using EncryptionParameters parms = DataAccessor.LoadParmsById(id);
        using SEALContext context = new(parms);
        using PublicKey publicKey = DataAccessor.LoadPublicKeyById(id, context);
        using SecretKey secretKey = DataAccessor.LoadSecretKeyById(id, context, password);

        // decrypt client signing encrypted key
        Pbkdf2KeyDeriver keyDeriver = new();
        AesEncryptor aes = new();
        byte[] key = keyDeriver.DeriveKey(password);
        byte[] clientSigningPrivateKey = aes.Decrypt(account.ClientSigningPrivateKeyEncrypted, key);

        // generate ciphertext and create transaction
        using MemoryStream ciphertextStream = new();
        EncryptionHelper.EncryptById(id, amount, context, publicKey, secretKey).Save(ciphertextStream);
        Transaction transaction = new(ciphertextStream.ToArray());

        // sign
        RsaSigner rsa = new();
        byte[] clientDigSig = rsa.Sign(clientSigningPrivateKey, transaction.Data);
        transaction.ClientDigSig = clientDigSig;

        // post to server
        Transaction returnedTransaction = await ApiAccessor.PostTransactionById(id, transaction);

        // verify signatures
        account.EnsureValid();

        // verify data will generate a ciphertext
        using MemoryStream stream = new(transaction.Data);
        using Ciphertext ciphertext = new();
        ciphertext.Load(context, stream);

        // save to client
        DataAccessor.AddTransactionById(account.Id, returnedTransaction, context);
    }

    public static async Task<long> GetBalanceById(int id, string password)
    {
        using EncryptionParameters parms = DataAccessor.LoadParmsById(id);
        using SEALContext context = new(parms);
        using SecretKey secretKey = DataAccessor.LoadSecretKeyById(id, context, password);
        using Ciphertext ciphertext = await ApiAccessor.GetBalanceById(id, context);
        long balance = EncryptionHelper.DecryptById(id, ciphertext, context, secretKey);
        return balance;
    }

    public static async Task DeleteAccountById(int id)
    {
        await ApiAccessor.DeleteAccountById(id);
        DataAccessor.DeleteAccountById(id);
    }
}
