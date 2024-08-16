using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary;

public class EncryptionHelper
{
    public EncryptionParameters Parms { get; }
    public SEALContext Context { get; }
    private Evaluator Evaluator { get; set; }

    public EncryptionHelper()
    {
        ulong polyModulusDegree = 4096;
        Parms = new(SchemeType.BFV);
        Parms.PolyModulusDegree = polyModulusDegree;
        Parms.CoeffModulus = CoeffModulus.BFVDefault(polyModulusDegree);

        // PlainModulus: 40961
        // Max plaintext value: (40961 - 1)/2 = 20480
        // Transaction range: ±$204.80
        Parms.PlainModulus = PlainModulus.Batching(Parms.PolyModulusDegree, 16);
        Context = new SEALContext(Parms);
        Evaluator = new Evaluator(Context);
    }

    public Ciphertext GetBalance(List<Ciphertext> transactions, RelinKeys relinKeys = null)
    {
        if (transactions == null || transactions.Count == 0)
        {
            return null;
        }

        Ciphertext balance = new(transactions[0]);

        for (int i = 1; i < transactions.Count; i++)
        {
            Evaluator.AddInplace(balance, transactions[i]);

            if (relinKeys != null)
            {
                Evaluator.RelinearizeInplace(balance, relinKeys);
            }
        }
        return balance;
    }
}
