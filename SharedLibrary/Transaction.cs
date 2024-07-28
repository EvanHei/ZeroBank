using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary;

public class Transaction
{
    public byte[] Data { get; set; }
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }

    public Transaction(byte[] data)
    {
        Data = data;
    }
}