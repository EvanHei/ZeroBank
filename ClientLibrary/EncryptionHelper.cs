using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public class EncryptionHelper
{
    public EncryptionParameters Parms { private get; set; }

    public ulong Decrypt(Ciphertext ciphertext)
    {
        if (Parms == null)
            throw new Exception();

        // TODO: move into the set function of parms
        SecretKey key = ClientConfig.DataAccessor.LoadSecretKey();
        using SEALContext context = new(Parms);
        using BatchEncoder encoder = new(context);
        using Decryptor decryptor = new(context, key);
        using Plaintext plaintext = new();
        List<ulong> result = new();

        decryptor.Decrypt(ciphertext, plaintext);
        encoder.Decode(plaintext, result);

        return result[0];
    }
}
