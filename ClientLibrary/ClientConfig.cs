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

        // verify server digital signature
        returnedAccount.EnsureValid();

        // sign
        RsaSigner rsa = new();
        byte[] clientDigSig = rsa.Sign(clientSigningPrivateKey, returnedAccount.SerializeMetadataToBytes());
        returnedAccount.ClientDigSig = clientDigSig;

        // send to server again
        await ApiAccessor.PostFullAccount(returnedAccount);

        // save to client
        DataAccessor.CreateAccount(returnedAccount);
    }

    public static async Task<List<Account>> GetAccounts()
    {
        // TODO: implement GetAccounts
        return new List<Account>();
    }

    public static async Task AddTransactionById(int id, long amount, string password)
    {
        // get encryption data
        using EncryptionParameters parms = DataAccessor.LoadParmsById(id);
        using SEALContext context = new(parms);
        using PublicKey publicKey = DataAccessor.LoadPublicKeyById(id, context);
        using SecretKey secretKey = DataAccessor.LoadSecretKeyById(id, context, password);

        // generate ciphertext and save to stream
        using MemoryStream ciphertextStream = new();
        EncryptionHelper.EncryptById(id, amount, context, publicKey, secretKey).Save(ciphertextStream);

        // TODO: add client dig sig

        Transaction transaction = new(ciphertextStream.ToArray(), new byte[0], new byte[0]);

        // post to server
        Transaction returnTransaction = await ApiAccessor.PostTransactionById(id, transaction);

        // TODO: verify server dig sig

        // save to client
        DataAccessor.AddTransactionById(id, transaction, context);
    }

    public static async Task<long> GetBalanceById(int id, string password)
    {
        // get encryption data
        using EncryptionParameters parms = DataAccessor.LoadParmsById(id);
        using SEALContext context = new(parms);
        using SecretKey secretKey = DataAccessor.LoadSecretKeyById(id, context, password);
        using Ciphertext ciphertext = await ApiAccessor.GetBalanceById(id, context);
        long balance = EncryptionHelper.DecryptById(id, ciphertext, context, secretKey);
        return balance;
    }

    public static async Task DeleteAccountById()
    {
        // TODO: implement DeleteAccountById
    }
}
