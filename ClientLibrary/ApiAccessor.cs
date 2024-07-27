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

        EncryptionParameters parms = new();
        Stream stream = await client.GetStreamAsync(Constants.ParmsUrl);
        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        parms.Load(memoryStream);
        return parms;
    }

    public async Task<List<Account>> GetAccounts()
    {
        if (!IsValidUrl(Constants.AccountsBaseUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.AccountsBaseUrl));
        }

        List<Account> accounts = await client.GetFromJsonAsync<List<Account>>(Constants.AccountsBaseUrl);
        return accounts;
    }

    public async Task<Account> PostAccount(Account account)
    {
        string url = Constants.AccountsBaseUrl;
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, account);
        response.EnsureSuccessStatusCode();
        Account returnedAccount = await response.Content.ReadFromJsonAsync<Account>();
        return returnedAccount;
    }

    public async Task DeleteAccountById(int id)
    {
        string url = $"{Constants.AccountsBaseUrl}/{id}";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Transaction> PostTransactionById(Transaction transaction, int id)
    {
        string url = $"{Constants.AccountsBaseUrl}/{id}/transaction";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, transaction);
        response.EnsureSuccessStatusCode();
        Transaction returnedTransaction = await response.Content.ReadFromJsonAsync<Transaction>();
        return returnedTransaction;
    }

    public async Task<Ciphertext?> GetBalanceById(SEALContext context, int id)
    {
        if (context == null)
        {
            throw new ArgumentNullException("SEALContext cannot be null.");
        }

        string url = $"{Constants.AccountsBaseUrl}/{id}/balance";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        Stream stream = await client.GetStreamAsync(url);
        MemoryStream memStream = new();
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