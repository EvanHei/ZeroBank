using Microsoft.Research.SEAL;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientLibrary;

public class ApiAccessor
{
    private static readonly HttpClient client = new();

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

    // test
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

    public async Task<Transaction> PostTransactionById(int id, Transaction transaction)
    {
        string url = $"{Constants.AccountsBaseUrl}/{id}/transaction";
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

        Transaction returnedTransaction = await response.Content.ReadFromJsonAsync<Transaction>();
        return returnedTransaction;
    }

    public async Task<Ciphertext?> GetBalanceById(int id, SEALContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "SEALContext cannot be null.");
        }

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

        using Stream stream = await response.Content.ReadAsStreamAsync();
        using MemoryStream memStream = new();
        stream.CopyTo(memStream);
        if (memStream.Length == 0)
        {
            return null;
        }

        memStream.Seek(0, SeekOrigin.Begin);

        Ciphertext ciphertext = new();
        ciphertext.Load(context, memStream);
        return ciphertext;
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