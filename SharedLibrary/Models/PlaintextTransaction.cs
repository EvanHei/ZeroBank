using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models;

public class PlaintextTransaction
{
    public long Amount { get; set; }
    public string FormattedAmount
    {
        get
        {
            NumberFormatInfo customFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            customFormat.CurrencyNegativePattern = 1;
            return (Amount * .01).ToString("C", customFormat);
        }
    }
    public int AccountId { get; set; }
    public DateTime Timestamp { get; set; }
}
