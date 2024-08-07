using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SharedLibrary.Models;

namespace ClientLibrary;

public class ApiAccessor
{
    private static readonly HttpClient client = new();

    public async Task Login(string username, string password)
    {
        string url = $"{Constants.UsersBaseUrl}/login";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

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
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

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
        if (!IsValidUrl(Constants.ParmsUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.ParmsUrl));
        }

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
        if (!IsValidUrl(Constants.AccountsBaseUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.AccountsBaseUrl));
        }

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
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

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
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, account);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task DeleteAccountById(int id)
    {
        string url = $"{Constants.AccountsBaseUrl}/{id}";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.DeleteAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }
    }

    public async Task<CiphertextTransaction> PostTransactionById(CiphertextTransaction transaction)
    {
        string url = $"{Constants.AccountsBaseUrl}/{transaction.AccountId}/transaction";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, transaction);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        CiphertextTransaction returnedTransaction = await response.Content.ReadFromJsonAsync<CiphertextTransaction>();
        return returnedTransaction;
    }

    public async Task<Stream> GetBalanceStreamById(int id)
    {
        string url = $"{Constants.AccountsBaseUrl}/{id}/balance";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        return await response.Content.ReadAsStreamAsync();
    }

    private static bool IsValidUrl(string url)
    {
        bool output = true;

        if (string.IsNullOrWhiteSpace(url))
            output = false;

        output = Uri.TryCreate(url, UriKind.Absolute, out Uri uriOutput) &&
            (uriOutput.Scheme == Uri.UriSchemeHttps || uriOutput.Scheme == Uri.UriSchemeHttp);

        return output;
    }
}