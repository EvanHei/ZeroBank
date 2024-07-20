using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }

    public SEALContext? Context { get; set; }
    private BatchEncoder? Encoder { get; set; }

    // TODO: test Decrypt
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
        List<long> result = new();

        decryptor.Decrypt(ciphertext, plaintext);
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
