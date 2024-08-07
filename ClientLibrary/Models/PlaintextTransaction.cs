using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Models;

public class PlaintextTransaction
{
    public long Amount { get; set; }
    public DateTime Timestamp { get; set; }
}
