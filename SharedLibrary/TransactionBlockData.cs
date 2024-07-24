using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary;

public class TransactionBlockData
{
    public byte[] Transaction { get; set; }
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }
    public DateTime Timestamp { get; } = DateTime.Now;

    public TransactionBlockData(byte[] transaction, byte[] clientDigSig)
    {
        Transaction = transaction;
        ClientDigSig = clientDigSig;
    }
}
