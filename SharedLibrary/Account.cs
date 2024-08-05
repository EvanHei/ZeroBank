using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace SharedLibrary;

public class Account
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public AccountType Type { get; set; }
    public DateTime DateCreated { get; private set; }
    public byte[] Parms { get; set; }
    public byte[] SEALPublicKey { get; set; }
    public byte[] SEALSecretKeyEncrypted { get; set; }
    public byte[] SEALRelinKeys { get; set; }
    public byte[] ClientSigningPublicKey { get; set; }
    public byte[] ClientSigningPrivateKeyEncrypted { get; set; }
    public byte[] ServerSigningPublicKey { get; set; }
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }
    public List<Transaction> Transactions { get; set; } = new();


    public Account(string name,
                   AccountType type,
                   DateTime dateCreated,
                   byte[] parms,
                   byte[] SEALPublicKey,
                   byte[] SEALSecretKeyEncrypted,
                   byte[] SEALRelinKeys,
                   byte[] clientSigningPublicKey,
                   byte[] clientSigningPrivateKeyEncrypted)
    {
        Name = name;
        Type = type;
        DateCreated = dateCreated;
        Parms = parms;
        this.SEALPublicKey = SEALPublicKey;
        this.SEALSecretKeyEncrypted = SEALSecretKeyEncrypted;
        this.SEALRelinKeys = SEALRelinKeys;
        ClientSigningPublicKey = clientSigningPublicKey;
        ClientSigningPrivateKeyEncrypted = clientSigningPrivateKeyEncrypted;
    }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

    // only these properties are signed, first by the server which initializes the Id and UserId, second by the client
    public byte[] SerializeMetadataToBytes()
    {
        var dataToSign = new
        {
            Id,
            UserId,
            Name,
            Type,
            DateCreated,
            Parms,
            SEALPublicKey,
            SEALSecretKeyEncrypted,
            SEALRelinKeys,
            ClientSigningPublicKey,
            ClientSigningPrivateKeyEncrypted,
            ServerSigningPublicKey
        };
        return JsonSerializer.SerializeToUtf8Bytes(dataToSign);
    }

    public void EnsureValid()
    {
        byte[] bytes = SerializeMetadataToBytes();
        RsaSigner rsa = new();

        // verify client and server digital signatures on the account
        if (ClientSigningPublicKey != null && ClientDigSig != null)
        {
            if (!rsa.Verify(ClientSigningPublicKey, ClientDigSig, bytes))
            {
                throw new CryptographicException("Client digital signature verification failed on the account.");
            }
        }

        if (ServerSigningPublicKey != null && ServerDigSig != null)
        {
            if (!rsa.Verify(ServerSigningPublicKey, ServerDigSig, bytes))
            {
                throw new CryptographicException("Server digital signature verification failed on the account.");
            }
        }

        // verify client and server digital signatures on each transaction
        foreach (Transaction transaction in Transactions)
        {
            if (ClientSigningPublicKey != null && transaction.ClientDigSig != null)
            {
                if (!rsa.Verify(ClientSigningPublicKey, transaction.ClientDigSig, transaction.Data))
                {
                    throw new CryptographicException("Client digital signature verification failed on a transaction.");
                }
            }

            if (ServerSigningPublicKey != null && transaction.ServerDigSig != null)
            {
                if (!rsa.Verify(ServerSigningPublicKey, transaction.ServerDigSig, transaction.Data))
                {
                    throw new CryptographicException("Server digital signature verification failed on a transaction.");
                }
            }
        }
    }
}
