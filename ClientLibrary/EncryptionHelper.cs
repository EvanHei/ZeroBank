using Microsoft.Research.SEAL;

namespace ClientLibrary;

public class EncryptionHelper
{
    private EncryptionParameters? parms;
    public EncryptionParameters Parms
    {
        get => parms;
        set
        {
            parms = value;
            Context = new SEALContext(Parms);
            Encoder = new BatchEncoder(Context);
            Evaluator = new Evaluator(Context);
        }
    }
    public SEALContext? Context { get; set; }
    private BatchEncoder? Encoder { get; set; }
    private Evaluator? Evaluator { get; set; }

    public Serializable<Ciphertext> Encrypt(long num)
    {
        PublicKey? publicKey = ClientConfig.DataAccessor.LoadPublicKey();
        if (publicKey == null)
        {
            throw new InvalidOperationException("No public key key found.");
        }

        SecretKey? secretKey = ClientConfig.DataAccessor.LoadSecretKey();
        if (secretKey == null)
        {
            throw new InvalidOperationException("No secret key found.");
        }

        if (Parms == null)
        {
            throw new InvalidOperationException("Encryption parameters must be set.");
        }

        using Plaintext absPlaintext = new();
        using Ciphertext absCiphertext = new();
        using Encryptor secretEncryptor = new(Context, secretKey);
        using Encryptor publicEncryptor = new(Context, publicKey);
        using Decryptor decryptor = new(Context, secretKey);
        ulong absNum = (ulong)Math.Abs(num);
        Encoder.Encode(new ulong[] { absNum }, absPlaintext);
        publicEncryptor.Encrypt(absPlaintext, absCiphertext);

        if (num < 0)
        {
            Evaluator.NegateInplace(absCiphertext);
        }

        using Plaintext plaintext = new();
        decryptor.Decrypt(absCiphertext, plaintext);
        Serializable<Ciphertext> ciphertext = secretEncryptor.EncryptSymmetric(plaintext);
        return ciphertext;
    }

    public long Decrypt(Ciphertext ciphertext)
    {
        if (Parms == null)
        {
            throw new InvalidOperationException("Encryption parameters must be set.");
        }

        SecretKey? secretKey = ClientConfig.DataAccessor.LoadSecretKey();
        if (secretKey == null)
        {
            throw new InvalidOperationException("No secret key found.");
        }

        using Plaintext plaintext = new();
        using Decryptor decryptor = new(Context, secretKey);
        decryptor.Decrypt(ciphertext, plaintext);
        List<long> result = new();
        Encoder.Decode(plaintext, result);
        return result[0];
    }

    public (PublicKey, SecretKey, Serializable<RelinKeys>) GenerateKeys()
    {
        using KeyGenerator keygen = new(Context);
        SecretKey secretKey = keygen.SecretKey;
        Serializable<RelinKeys> relinKeys = keygen.CreateRelinKeys();
        keygen.CreatePublicKey(out PublicKey publicKey);
        return (publicKey, secretKey, relinKeys);
    }
}
