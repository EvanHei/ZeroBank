using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary;

public class DataAccessor
{
    public void SaveSecretKey(SecretKey secretKey)
    {
    }

    public SecretKey LoadSecretKey()
    {
        return new SecretKey();
    }
}