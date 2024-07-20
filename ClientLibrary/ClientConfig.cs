using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public static class ClientConfig
{
    public static DataAccessor DataAccessor { get; set; } = new DataAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();
}
