using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models;

public class CloseAccountRequest
{
    public CloseAccountRequest(Account account, byte[] key)
    {
        Account = account;
        Key = key;
    }

    public Account Account {  get; set; }
    public byte[] Key { get; set; }
}
