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

    public async Task<List<Blockchain>> GetAccounts()
    {
        if (!IsValidUrl(Constants.AccountsBaseUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.AccountsBaseUrl));
        }

        List<Blockchain> accounts = await client.GetFromJsonAsync<List<Blockchain>>(Constants.AccountsBaseUrl);
        return accounts;
    }

    public async Task<GenesisBlockData> PostAccount(GenesisBlockData genesisBlockData)
    {
        string url = Constants.AccountsBaseUrl;
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, genesisBlockData);
        response.EnsureSuccessStatusCode();
        GenesisBlockData returnedGenesisBlockData = await response.Content.ReadFromJsonAsync<GenesisBlockData>();
        return returnedGenesisBlockData;
    }

    public async Task DeleteAccountById(int accountId)
    {
        string url = $"{Constants.AccountsBaseUrl}/{accountId}";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TransactionBlockData> PostTransactionById(TransactionBlockData transactionBlockData, int accountId)
    {
        string url = $"{Constants.AccountsBaseUrl}/{accountId}/transaction";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        HttpResponseMessage response = await client.PostAsJsonAsync(url, transactionBlockData);
        response.EnsureSuccessStatusCode();
        TransactionBlockData returnedTransactionBlockData = await response.Content.ReadFromJsonAsync<TransactionBlockData>();
        return returnedTransactionBlockData;
    }

    public async Task<Ciphertext?> GetBalanceById(int accountId)
    {
        string url = $"{Constants.AccountsBaseUrl}/{accountId}/balance";
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("Invalid URL", nameof(url));
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            throw new InvalidOperationException("SEALContext must be set.");
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
        ciphertext.Load(ClientConfig.EncryptionHelper.Context, memStream);
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