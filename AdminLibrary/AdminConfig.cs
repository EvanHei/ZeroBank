using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public static class AdminConfig
{
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public static async Task CreateAdmin(string username, string password)
    {
        throw new NotImplementedException();
    }

    public static async Task DeleteAdmin(string username, string password)
    {
        throw new NotImplementedException();
    }

    // TODO: add a user data retrieval method
}
