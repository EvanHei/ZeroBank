using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SharedLibrary;

public class ApiAccessor
{
    private static readonly HttpClient client = new();

    public async Task Login(string username, string password)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.UserLoginUrl, new Credentials(username, password));
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }

        // retrieve and add token as a header
        string token = await response.Content.ReadFromJsonAsync<string>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task SignUp(string username, string password)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.UserSignUpUrl, new Credentials(username, password));
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
        HttpResponseMessage response = await client.GetAsync(Constants.AccountBaseUrl);
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
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AccountPartialAccountUrl, account);
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
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AccountFullAccountUrl, account);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task<CiphertextTransaction> PostTransaction(CiphertextTransaction transaction)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AccountTransactionUrl, transaction);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        CiphertextTransaction returnedTransaction = await response.Content.ReadFromJsonAsync<CiphertextTransaction>();
        return returnedTransaction;
    }

    public async Task<Account> CloseAccount(Account account, byte[] key)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AccountCloseUrl, new UserCloseAccountRequest(account, key));
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        Account closedAccount = await response.Content.ReadFromJsonAsync<Account>();
        return closedAccount;
    }
}