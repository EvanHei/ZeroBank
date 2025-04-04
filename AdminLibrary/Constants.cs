using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public static class Constants
{
    public static readonly string AdminLoginUrl = "https://localhost:7188/api/Admin/login";
    public static readonly string AdminCreateUrl = "https://localhost:7188/api/Admin/create";
    public static readonly string AdminDeleteUrl = "https://localhost:7188/api/Admin/delete";
    public static readonly string GetUserAccountsUrl = "https://localhost:7188/api/Admin/accounts";
    public static readonly string GetUsersUrl = "https://localhost:7188/api/Admin/users";
    public static readonly string GetKeysUrl = "https://localhost:7188/api/Admin/keys";
}
