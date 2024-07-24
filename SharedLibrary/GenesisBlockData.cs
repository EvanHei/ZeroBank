using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary;

public class GenesisBlockData
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public AccountType AccountType { get; set; }
    //public byte[] RelinKeys { get; set; }
    //public byte[] ClientEncryptedTransactionSecretKey { get; set; }
    //public byte[] ClientEncryptedSigningSecretKey { get; set; }
    //public byte[] ClientPublicKey { get; set; }
    //public byte[] ServerPublicKey { get; set; }
    public byte[] ClientDigSig { get; set; }
    public byte[] ServerDigSig { get; set; }

    public GenesisBlockData(string accountName, AccountType accountType)
    {
        AccountName = accountName;
        AccountType = accountType;
    }
}
