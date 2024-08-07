using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibrary.Models;

public class CiphertextTransaction
{
    public byte[] Ciphertext { get; set; }
    public int AccountId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }

    public CiphertextTransaction(byte[] ciphertext, int accountId)
    {
        Ciphertext = ciphertext;
        AccountId = accountId;
    }

    // only these properties are signed, first by the client, second by the server
    public byte[] SerializeMetadataToBytes()
    {
        var dataToSign = new
        {
            Ciphertext,
            AccountId,
            Timestamp
        };
        return JsonSerializer.SerializeToUtf8Bytes(dataToSign);
    }
}