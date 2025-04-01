using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public static class AdminConfig
{
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task AdminLogin(Credentials adminCredentials)
    {
        await ApiAccessor.AdminLogin(adminCredentials);
    }

    public static async Task AdminCreate(Credentials adminCredentials)
    {
        await ApiAccessor.AdminCreate(adminCredentials);
        await ApiAccessor.AdminLogin(adminCredentials);
    }

    public static async Task AdminDelete(Credentials adminCredentials)
    {
        await ApiAccessor.AdminDelete(adminCredentials);
    }

    public static ulong LoadPlainModulus(Account account)
    {
        EncryptionParameters parms = new();
        using MemoryStream stream = new(account.Parms);
        parms.Load(stream);
        return parms.PlainModulus.Value;
    }
}
