using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task PostRelinKeys(RelinKeys relinKeys)
    {
        if (!IsValidUrl(Constants.RelinKeysUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.RelinKeysUrl));
        }

        MemoryStream stream = new();
        relinKeys.Save(stream);
        using StreamContent content = new(stream);
        HttpResponseMessage response = await client.PostAsync(Constants.RelinKeysUrl, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task PostTransaction(Serializable<Ciphertext> transaction)
    {
        if (!IsValidUrl(Constants.TransactionUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.TransactionUrl));
        }

        MemoryStream stream = new();
        transaction.Save(stream);
        using StreamContent content = new(stream);
        HttpResponseMessage response = await client.PostAsync(Constants.TransactionUrl, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Ciphertext?> GetBalance()
    {
        if (!IsValidUrl(Constants.BalanceUrl))
        {
            throw new ArgumentException("Invalid URL", nameof(Constants.BalanceUrl));
        }

        if (ClientConfig.EncryptionHelper.Context == null)
        {
            throw new InvalidOperationException("SEALContext must be set.");
        }

        Stream stream = await client.GetStreamAsync(Constants.BalanceUrl);
        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);

        if (memoryStream.Length == 0)
        {
            return null;
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        Ciphertext ciphertext = new();
        ciphertext.Load(ClientConfig.EncryptionHelper.Context, memoryStream);
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