using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary;

public static class ServerConfig
{
    public static DataAccessor DataAccessor { get; set; } = new DataAccessor();
    public static EncryptionHelper EncryptionHelper { get; set; } = new EncryptionHelper();
}
