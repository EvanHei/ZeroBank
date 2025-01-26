using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public static class AdminConfig
{
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task AdminLogin(string username, string password)
    {
        await ApiAccessor.AdminLogin(username, password);
    }

    public static async Task AdminCreate(string username, string password)
    {
        await ApiAccessor.AdminCreate(username, password);
        await ApiAccessor.AdminLogin(username, password);
    }

    public static async Task AdminDelete(string username, string password)
    {
        await ApiAccessor.AdminDelete(username, password);
    }

    // TODO: add a user data retrieval method
}
