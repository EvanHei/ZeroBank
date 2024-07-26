using Microsoft.Research.SEAL;
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

        // adjust as needed
        Parms.PlainModulus = PlainModulus.Batching(Parms.PolyModulusDegree, 30);
        Context = new SEALContext(Parms);
        Evaluator = new Evaluator(Context);
    }

    public Ciphertext? GetBalance(List<Ciphertext> transactions, int id, RelinKeys? relinKeys = null)
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
