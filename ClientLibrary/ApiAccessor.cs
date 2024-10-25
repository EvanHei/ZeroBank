using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClientLibrary;

public class ApiAccessor
{
    private static readonly HttpClient client = new();

    public async Task Login(string username, string password)
    {
        string url = $"{Constants.UsersBaseUrl}/login";

        UserCredentials credentials = new(username, password);
        HttpResponseMessage response = await client.PostAsJsonAsync(url, credentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }

        string token = await response.Content.ReadFromJsonAsync<string>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task SignUp(string username, string password)
    {
        string url = $"{Constants.UsersBaseUrl}/signup";

        UserCredentials credentials = new(username, password);
        HttpResponseMessage response = await client.PostAsJsonAsync(url, credentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task<EncryptionParameters> GetEncryptionParameters()
    {

        HttpResponseMessage response = await client.GetAsync(Constants.ParmsUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        using Stream stream = await response.Content.ReadAsStreamAsync();
        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        EncryptionParameters parms = new();
        parms.Load(memoryStream);
        return parms;
    }

    public async Task<List<Account>> GetAccounts()
    {

        HttpResponseMessage response = await client.GetAsync(Constants.AccountsBaseUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        List<Account> accounts = await response.Content.ReadFromJsonAsync<List<Account>>();
        return accounts;
    }

    public async Task<Account> PostPartialAccount(Account account)
    {
        string url = $"{Constants.AccountsBaseUrl}/partial-account";

        HttpResponseMessage response = await client.PostAsJsonAsync(url, account);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }

        Account returnedAccount = await response.Content.ReadFromJsonAsync<Account>();
        return returnedAccount;
    }

    public async Task PostFullAccount(Account account)
    {
        string url = $"{Constants.AccountsBaseUrl}/full-account";

        HttpResponseMessage response = await client.PostAsJsonAsync(url, account);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task<Account> CloseAccount(Account account, byte[] key)
    {
        string url = $"{Constants.AccountsBaseUrl}/close";

        HttpResponseMessage response = await client.PostAsJsonAsync(url, new CloseAccountRequest(account, key));
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        Account closedAccount = await response.Content.ReadFromJsonAsync<Account>();
        return closedAccount;
    }

    public async Task<CiphertextTransaction> PostTransaction(CiphertextTransaction transaction)
    {
        string url = $"{Constants.AccountsBaseUrl}/transaction";

        HttpResponseMessage response = await client.PostAsJsonAsync(url, transaction);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        CiphertextTransaction returnedTransaction = await response.Content.ReadFromJsonAsync<CiphertextTransaction>();
        return returnedTransaction;
    }

    public async Task<Stream> GetBalanceStream(int accountId)
    {
        string url = $"{Constants.AccountsBaseUrl}/{accountId}/balance";

        HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        return await response.Content.ReadAsStreamAsync();
    }
}