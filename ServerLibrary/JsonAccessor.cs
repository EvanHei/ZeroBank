using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System.IO;
using System.Text.Json;

namespace ServerLibrary;

public class JsonAccessor
{
    public JsonAccessor()
    {
        Directory.CreateDirectory(Constants.ServerDirectoryPath);
        Directory.CreateDirectory(Constants.AccountsDirectoryPath);
        Directory.CreateDirectory(Constants.PrivateKeysDirectoryPath);

        if (!File.Exists(Constants.AdminsFilePath))
        {
            // create admin with default credentials
            Credentials adminCredentials = new("admin", "admin");
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(adminCredentials.Password);
            User defaultAdmin = new(adminCredentials.Username, passwordHash);

            string json = JsonSerializer.Serialize(new List<User>());
            File.WriteAllText(Constants.AdminsFilePath, json);
            CreateAdmin(defaultAdmin);
        }

        if (!File.Exists(Constants.UsersFilePath))
        {
            string json = JsonSerializer.Serialize(new List<User>());
            File.WriteAllText(Constants.UsersFilePath, json);
        }

        if (!File.Exists(Constants.UserPrivateKeysFilePath))
        {
            string json = JsonSerializer.Serialize(new Dictionary<int, string>());
            File.WriteAllText(Constants.UserPrivateKeysFilePath, json);
        }
    }

    public User LoadUser(Credentials userCredentials)
    {
        User user = LoadUsers().FirstOrDefault(u => u.Username.Equals(userCredentials.Username, StringComparison.OrdinalIgnoreCase));

        // verify the password
        if (user != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }

    public List<User> LoadUsers()
    {
        if (!File.Exists(Constants.UsersFilePath))
        {
            throw new FileNotFoundException($"File not found: {Constants.UsersFilePath}");
        }

        string json = File.ReadAllText(Constants.UsersFilePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json);
        return users;
    }

    public void CreateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentException("User cannot be null.");
        }

        List<User> users = LoadUsers();

        if (users.Any(u => u.Username == user.Username))
        {
            throw new ArgumentException("Username taken.");
        }

        users.Add(user);
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.UsersFilePath, json);
    }

    public User LoadAdmin(Credentials userCredentials)
    {
        User admin = LoadAdmins().FirstOrDefault(u => u.Username.Equals(userCredentials.Username, StringComparison.OrdinalIgnoreCase));

        // verify the password
        if (admin != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, admin.PasswordHash))
        {
            return admin;
        }

        return null;
    }

    public List<User> LoadAdmins()
    {
        if (!File.Exists(Constants.AdminsFilePath))
        {
            throw new FileNotFoundException($"File not found: {Constants.AdminsFilePath}");
        }

        string json = File.ReadAllText(Constants.AdminsFilePath);
        List<User> admins = JsonSerializer.Deserialize<List<User>>(json);
        return admins;
    }

    public void CreateAdmin(User admin)
    {
        if (admin == null)
        {
            throw new ArgumentException("User cannot be null.");
        }

        List<User> admins = LoadAdmins();

        if (admins.Any(u => u.Username == admin.Username))
        {
            throw new ArgumentException("Admin username taken.");
        }

        admins.Add(admin);
        string json = JsonSerializer.Serialize(admins, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.AdminsFilePath, json);
    }

    public void DeleteAdmin(User admin)
    {
        if (admin == null)
        {
            throw new ArgumentException("Admin cannot be null.");
        }

        List<User> admins = LoadAdmins();

        User adminToDelete = admins.FirstOrDefault(u => u.Username == admin.Username);
        if (adminToDelete == null)
        {
            throw new ArgumentException("Admin not found.");
        }

        admins.Remove(adminToDelete);
        string json = JsonSerializer.Serialize(admins, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.AdminsFilePath, json);
    }

    public RelinKeys LoadRelinKeys(int id)
    {
        Account account = LoadAccount(id);
        byte[] relinKeysBytes = account.SEALRelinKeys;
        using MemoryStream stream = new(relinKeysBytes);
        RelinKeys relinKeys = new();
        relinKeys.Load(ServerConfig.EncryptionHelper.Context, stream);
        return relinKeys;
    }

    public Account CreateAccount(Account account, byte[] serverSigningPrivateKey)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }

        // save server signing private key
        string path = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Id + ".bin");
        File.WriteAllBytes(path, serverSigningPrivateKey);

        // save partial account
        SaveAccount(account);
        return account;
    }

    public void SaveAccount(Account account)
    {
        string json = account.SerializeToJson();
        string path = Path.Combine(Constants.AccountsDirectoryPath, account.Id + ".json");
        File.WriteAllText(path, json);
    }

    public List<Account> LoadUserAccounts(int userId)
    {
        List <Account> userAccounts = LoadAllAccounts().Where(a => a.UserId == userId).ToList();
        return userAccounts;
    }

    public List<Account> LoadAllAccounts()
    {
        if (!Directory.Exists(Constants.AccountsDirectoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {Constants.AccountsDirectoryPath}");
        }

        List<Account> accounts = new();
        string[] accountFiles = Directory.GetFiles(Constants.AccountsDirectoryPath);
        foreach (string accountFile in accountFiles)
        {
            string json = File.ReadAllText(accountFile);
            Account account = JsonSerializer.Deserialize<Account>(json);
            accounts.Add(account);
        }
        return accounts;
    }

    public Account LoadAccount(int id)
    {
        Account account = LoadAllAccounts().Where(a => a.Id == id).FirstOrDefault() ?? throw new InvalidOperationException("Account not found.");
        return account;
    }

    public void DeleteAccount(int id)
    {
        // delete the account file
        Account account = LoadAccount(id);
        string accountPath = Path.Combine(Constants.AccountsDirectoryPath, account.Id + ".json");
        File.Delete(accountPath);

        // delete the key file
        string keyPath = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Id + ".bin");
        File.Delete(keyPath);
    }

    public void CloseAccount(Account account, byte[] key)
    {
        SaveAccount(account);
        SaveUserPrivateKey(account.Id, key);
    }

    public List<Ciphertext> LoadTransactions(int id)
    {
        Account account = LoadAccount(id);
        List<Ciphertext> transactions = new();

        foreach (CiphertextTransaction transaction in account.Transactions)
        {
            try
            {
                using MemoryStream stream = new(transaction.Ciphertext);
                Ciphertext ciphertext = new();
                ciphertext.Load(ServerConfig.EncryptionHelper.Context, stream);
                transactions.Add(ciphertext);
            }
            catch (Exception ex)
            {
                // TODO: add logging
                continue;
            }        
        }
        return transactions;
    }

    public CiphertextTransaction AddTransaction(CiphertextTransaction transaction)
    {
        Account account = LoadAccount(transaction.AccountId);
        account.Transactions.Add(transaction);
        SaveAccount(account);
        return transaction;
    }

    public byte[] LoadSigningKey(int id)
    {
        Account account = LoadAccount(id);
        string path = Path.Combine(Constants.PrivateKeysDirectoryPath, account.Id + ".bin");
        return File.ReadAllBytes(path);
    }

    private Dictionary<int, string> LoadUserPrivateKeys()
    {
        if (!File.Exists(Constants.UserPrivateKeysFilePath))
        {
            throw new FileNotFoundException($"File not found: {Constants.UserPrivateKeysFilePath}");
        }

        string json = File.ReadAllText(Constants.UserPrivateKeysFilePath);
        Dictionary<int, string> keys = JsonSerializer.Deserialize<Dictionary<int, string>>(json);
        return keys;
    }

    public void SaveUserPrivateKey(int accountId, byte[] key)
    {
        Dictionary<int, string> keys = LoadUserPrivateKeys();
        string keyString = Convert.ToBase64String(key);
        keys[accountId] = keyString;
        string json = JsonSerializer.Serialize(keys, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Constants.UserPrivateKeysFilePath, json);
    }

    public byte[] LoadUserPrivateKey(int accountId)
    {
        Dictionary<int, string> keys = LoadUserPrivateKeys();

        if (!keys.TryGetValue(accountId, out string encodedKey))
        {
            throw new KeyNotFoundException("Key not found for the specified account ID.");
        }

        return Convert.FromBase64String(encodedKey);
    }
}
