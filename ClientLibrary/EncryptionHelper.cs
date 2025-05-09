﻿using Microsoft.Research.SEAL;
using SharedLibrary;

namespace ClientLibrary;

public class EncryptionHelper
{
    public Serializable<Ciphertext> Encrypt(long amount, SEALContext context, PublicKey publicKey, SecretKey secretKey)
    {
        if (context == null)
        {
            throw new ArgumentNullException("SEALContext cannot be null.");
        }

        if (publicKey == null)
        {
            throw new ArgumentNullException("Public key cannot be null.");
        }

        if (secretKey == null)
        {
            throw new ArgumentNullException("Secret key cannot be null.");
        }

        using BatchEncoder encoder = new(context);
        using Evaluator evaluator = new(context);
        using Encryptor secretEncryptor = new(context, secretKey);
        using Encryptor publicEncryptor = new(context, publicKey);
        using Decryptor decryptor = new(context, secretKey);

        // convert to positive number
        ulong absNum = (ulong)Math.Abs(amount);

        using Plaintext absPlaintext = new();
        using Ciphertext absCiphertext = new();
        encoder.Encode(new ulong[] { absNum }, absPlaintext);
        publicEncryptor.Encrypt(absPlaintext, absCiphertext);

        // mulitply ciphertext by -1 if the amount is negative
        if (amount < 0)
        {
            evaluator.NegateInplace(absCiphertext);
        }

        // encrypt symmetrically to reduce the size
        using Plaintext plaintext = new();
        decryptor.Decrypt(absCiphertext, plaintext);
        Serializable<Ciphertext> ciphertext = secretEncryptor.EncryptSymmetric(plaintext);
        return ciphertext;
    }

    public long Decrypt(Ciphertext ciphertext, SEALContext context, SecretKey secretKey)
    {
        if (context == null)
        {
            throw new ArgumentNullException("SEALContext cannot be null.");
        }

        if (secretKey == null)
        {
            throw new ArgumentNullException("Secret key cannot be null.");
        }

        using BatchEncoder encoder = new(context);
        using Plaintext plaintext = new();
        using Decryptor decryptor = new(context, secretKey);
        decryptor.Decrypt(ciphertext, plaintext);
        List<long> result = new();
        encoder.Decode(plaintext, result);
        return result[0];
    }

    public (PublicKey SEALPublicKey, SecretKey SEALSecretKey, Serializable<RelinKeys> SEALRelinKeys, byte[] publicSigningKey, byte[] privateSigningKey) GenerateKeys(EncryptionParameters parms)
    {
        // signing keys
        RsaSigner rsa = new();
        (byte[] publicSigningKey, byte[] privateSigningKey) = rsa.GenerateKeyPair();

        // SEAL keys
        using SEALContext context = new(parms);
        using KeyGenerator keygen = new(context);
        SecretKey SEALSecretKey = keygen.SecretKey;
        Serializable<RelinKeys> SEALRelinKeys = keygen.CreateRelinKeys();
        keygen.CreatePublicKey(out PublicKey SEALPublicKey);

        return (SEALPublicKey, SEALSecretKey, SEALRelinKeys, publicSigningKey, privateSigningKey);
    }
}
