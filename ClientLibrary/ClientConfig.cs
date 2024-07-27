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
        (PublicKey publicKey, SecretKey secretKey, Serializable<RelinKeys> relinKeys) = EncryptionHelper.GenerateKeys(parms);

        // save keys to streams
        using MemoryStream publicKeyStream = new();
        using MemoryStream secretKeyStream = new();
        using MemoryStream relinKeysStream = new();
        publicKey.Save(publicKeyStream);
        secretKey.Save(secretKeyStream);
        relinKeys.Save(relinKeysStream);

        // TODO: encrypt secret key bytes
        byte[] secretKeyBytes = secretKeyStream.ToArray();
        Pbkdf2KeyDeriver keyDeriver = new();
        AesEncryptor aes = new();
        byte[] key = keyDeriver.DeriveKey(password, new byte[0]);
        byte[] encryptedSecretKeyBytes = aes.Encrypt(secretKeyBytes, key);

        // create account
        Account account = new(name, type, DateTime.Now, parmsStream.ToArray(), publicKeyStream.ToArray(), encryptedSecretKeyBytes, relinKeysStream.ToArray());

        // post to server
        Account returnedAccount = await ApiAccessor.PostAccount(account);

        // save to client
        DataAccessor.CreateAccount(returnedAccount);
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
        await ApiAccessor.PostTransactionById(transaction, id);

        // save to client
        DataAccessor.AddTransactionById(id, transaction, context);
    }

    public static async Task<long> GetBalanceById(int id, string password)
    {
        // get encryption data
        using EncryptionParameters parms = DataAccessor.LoadParmsById(id);
        using SEALContext context = new(parms);
        using SecretKey secretKey = DataAccessor.LoadSecretKeyById(id, context, password);
        using Ciphertext ciphertext = await ApiAccessor.GetBalanceById(context, id);
        long balance = EncryptionHelper.DecryptById(id, ciphertext, context, secretKey);
        return balance;
    }
}
