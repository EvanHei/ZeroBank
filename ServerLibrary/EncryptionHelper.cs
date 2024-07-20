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
    public RelinKeys RelinKeys { get; set; } = new RelinKeys();
    public SEALContext Context { get; }
    private Evaluator Evaluator { get; set; }

    public EncryptionHelper()
    {
        ulong polyModulusDegree = 4096;
        Parms = new EncryptionParameters(SchemeType.BFV);
        Parms.PolyModulusDegree = polyModulusDegree;
        Parms.CoeffModulus = CoeffModulus.BFVDefault(polyModulusDegree);
        Parms.PlainModulus = PlainModulus.Batching(Parms.PolyModulusDegree, 30);

        Context = new SEALContext(Parms);
        Evaluator = new Evaluator(Context);
        RelinKeys = ServerConfig.DataAccessor.LoadRelinKeys();
    }

    // TODO: test
    public Ciphertext? GetBalance()
    {
        List<Ciphertext> transactions = ServerConfig.DataAccessor.LoadTransactions();

        if (transactions.Count == 0)
        {
            return null;
        }

        Ciphertext balance = new(transactions[0]);

        for (int i = 1; i < transactions.Count; i++)
        {
            Evaluator.AddInplace(balance, transactions[i]);

            if (RelinKeys != null)
            {
                Evaluator.RelinearizeInplace(balance, RelinKeys);
            }
        }

        return balance;
    }
}
