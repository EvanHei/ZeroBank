using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public class ApiAccessor
{
    // TODO: implement GetEncryptionParameters
    public EncryptionParameters GetEncryptionParameters()
    {
        // TODO: temp code for testing
        ulong polyModulusDegree = 4096;
        EncryptionParameters parms = new(SchemeType.BFV);
        parms.PolyModulusDegree = polyModulusDegree;
        parms.CoeffModulus = CoeffModulus.BFVDefault(polyModulusDegree);
        parms.PlainModulus = PlainModulus.Batching(parms.PolyModulusDegree, 30);

        return parms;
    }

    // TODO: implement PostRelinKeys
    public void PostRelinKeys(Serializable<RelinKeys> relinKeys)
    {
    }

    // TODO: implement PostTransaction
    public void PostTransaction(Ciphertext transaction)
    {
    }

    // TODO: implement GetBalance
    public Ciphertext GetBalance()
    {
        return new Ciphertext();
    }
}