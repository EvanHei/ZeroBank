using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models;

public class AdminCloseAccountRequest
{
    public AdminCloseAccountRequest(int accountId)
    {
        AccountId = accountId;
    }

    public int AccountId { get; set; }
}
