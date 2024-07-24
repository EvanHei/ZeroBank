using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary;

public class TransactionBlockData
{
    public byte[] Transaction { get; set; }
    public DateTime Timestamp { get; set; }
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }

    public TransactionBlockData(byte[] transaction, byte[] clientDigSig, DateTime timestamp)
    {
        Transaction = transaction;
        ClientDigSig = clientDigSig;
        Timestamp = timestamp;
    }
}
