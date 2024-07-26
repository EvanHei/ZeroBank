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
    public DateTime DateCreated { get; } = DateTime.Now;
    public byte[] RelinKeys { get; set; }
    public byte[] ClientSigningPublicKey { get; set; }
    public byte[] ClientEncryptedSigningPrivateKey { get; set; }
    public byte[] ClientSEALPublicKey { get; set; }
    public byte[] ClientEncryptedSEALPrivateKey { get; set; }
    public byte[] ServerPublicKey { get; set; }
    public byte[] ServerDigSig { get; set; }
    public byte[] ClientDigSig { get; set; }
    public List<Transaction> Transactions { get; set; } = new();

    public Account(string name, AccountType type, byte[] relinKeys)
    {
        Name = name;
        Type = type;
        RelinKeys = relinKeys;
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
