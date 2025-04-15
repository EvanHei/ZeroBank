using Microsoft.Research.SEAL;
using ServerLibrary;
using System;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Tests;

public class EncryptionHelperTests
{
    private Ciphertext Encrypt(long value, SEALContext context, PublicKey publicKey)
    {
        using BatchEncoder encoder = new(context);
        using Encryptor encryptor = new(context, publicKey);

        ulong abs = (ulong)Math.Abs(value);
        using Plaintext plain = new();
        encoder.Encode(new ulong[] { abs }, plain);

        Ciphertext ciphertext = new();
        encryptor.Encrypt(plain, ciphertext);

        if (value < 0)
        {
            using Evaluator evaluator = new(context);
            evaluator.NegateInplace(ciphertext);
        }

        return ciphertext;
    }

    private long Decrypt(Ciphertext ciphertext, SEALContext context, SecretKey secretKey)
    {
        using BatchEncoder encoder = new(context);
        using Decryptor decryptor = new(context, secretKey);
        using Plaintext plain = new();
        decryptor.Decrypt(ciphertext, plain);

        List<long> output = new();
        encoder.Decode(plain, output);
        return output[0];
    }

    [Fact]
    public void GetBalance_MultipleTransactions_ReturnsCorrectSum()
    {
        EncryptionHelper helper = new();
        using KeyGenerator keygen = new(helper.Context);
        keygen.CreatePublicKey(out PublicKey publicKey);
        SecretKey secretKey = keygen.SecretKey;

        List<long> amounts = new() { 100, -50, 20 }; // Expected balance: 70
        List<Ciphertext> encrypted = new();

        foreach (long amount in amounts)
        {
            encrypted.Add(Encrypt(amount, helper.Context, publicKey));
        }

        using Ciphertext balance = helper.GetBalance(encrypted);
        long result = Decrypt(balance, helper.Context, secretKey);

        Assert.Equal(70, result);
    }

    [Fact]
    public void GetBalance_WithRelinKeys_Success()
    {
        EncryptionHelper helper = new();
        using KeyGenerator keygen = new(helper.Context);
        keygen.CreatePublicKey(out PublicKey publicKey);
        SecretKey secretKey = keygen.SecretKey;

        Serializable<RelinKeys> serializableRelinKeys = keygen.CreateRelinKeys();
        using MemoryStream relinStream = new();
        serializableRelinKeys.Save(relinStream);
        relinStream.Seek(0, SeekOrigin.Begin);
        RelinKeys relinKeys = new();
        relinKeys.Load(helper.Context, relinStream);

        List<long> amounts = new() { -40, -30, 10 }; // Expected balance: -60
        List<Ciphertext> encrypted = new();

        foreach (long amount in amounts)
        {
            encrypted.Add(Encrypt(amount, helper.Context, publicKey));
        }

        using Ciphertext balance = helper.GetBalance(encrypted, relinKeys);
        long result = Decrypt(balance, helper.Context, secretKey);

        Assert.Equal(-60, result);
    }

    [Fact]
    public void GetBalance_EmptyList_ReturnsNull()
    {
        EncryptionHelper helper = new();
        Ciphertext balance = helper.GetBalance(new List<Ciphertext>());
        Assert.Null(balance);
    }
}