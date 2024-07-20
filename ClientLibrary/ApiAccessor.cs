using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public class ApiAccessor
{
    public EncryptionParameters GetEncryptionParameters()
    {
        return new EncryptionParameters();
    }

    public void PostRelinKeys(Serializable<RelinKeys> relinKeys)
    {
    }

    public void PostTransaction(Ciphertext transaction)
    {
    }

    public Ciphertext GetBalance()
    {
        return new Ciphertext();
    }
}