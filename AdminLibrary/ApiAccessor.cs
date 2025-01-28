using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public class ApiAccessor
{
    private static readonly HttpClient client = new();

    public async Task AdminLogin(string username, string password)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminLoginUrl, new Credentials(username, password));
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }

        // retrieve and add token as a header
        string token = await response.Content.ReadFromJsonAsync<string>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task AdminCreate(string username, string password)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminCreateUrl, new Credentials(username, password));
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task AdminDelete(string username, string password)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminDeleteUrl, new Credentials(username, password));
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task<List<Account>> GetUserAccounts()
    {
        HttpResponseMessage response = await client.GetAsync(Constants.GetUserAccountsUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        List<Account> accounts = await response.Content.ReadFromJsonAsync<List<Account>>();
        return accounts;
    }
}
