using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace SharedLibrary;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AccountType Type { get; set; }
    public DateTime DateCreated { get; private set; }
    public byte[] Parms { get; set; }
    public byte[] SEALPublicKey { get; set; }
    public byte[] SEALSecretKeyEncrypted { get; set; }
    public byte[] SEALRelinKeys { get; set; }
    public byte[] ClientSigningPublicKey { get; set; }
    public byte[] ClientEncryptedSigningPrivateKey { get; set; }
    public byte[] ServerPublicKey { get; set; }
    public byte[] ServerDigSig { get; set; }
    public byte[] ClientDigSig { get; set; }
    public List<Transaction> Transactions { get; set; } = new();

    public Account(string name, AccountType type, DateTime dateCreated, byte[] parms, byte[] SEALPublicKey, byte[] SEALSecretKeyEncrypted, byte[] SEALRelinKeys)
    {
        Name = name;
        Type = type;
        DateCreated = dateCreated;
        Parms = parms;
        this.SEALPublicKey = SEALPublicKey;
        this.SEALSecretKeyEncrypted = SEALSecretKeyEncrypted;
        this.SEALRelinKeys = SEALRelinKeys;
    }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

    public void EnsureValid()
    {
        // for each transaction, verify the signatures with RSA and throw exception if it doesn't work
    }
}
