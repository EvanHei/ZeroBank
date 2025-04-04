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

    public async Task AdminLogin(Credentials adminCredentials)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminLoginUrl, adminCredentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }

        // retrieve and add token as a header
        string token = await response.Content.ReadFromJsonAsync<string>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task AdminCreate(Credentials adminCredentials)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminCreateUrl, adminCredentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task AdminDelete(Credentials adminCredentials)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminDeleteUrl, adminCredentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task<List<Account>> GetAccounts()
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

    public async Task<List<User>> GetUsers()
    {
        HttpResponseMessage response = await client.GetAsync(Constants.GetUsersUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        List<User> users = await response.Content.ReadFromJsonAsync<List<User>>();
        return users;
    }

    public async Task<List<User>> GetAdmins()
    {
        HttpResponseMessage response = await client.GetAsync(Constants.GetAdminsUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        List<User> admins = await response.Content.ReadFromJsonAsync<List<User>>();
        return admins;
    }

    public async Task<Dictionary<int, string>> GetKeys()
    {
        HttpResponseMessage response = await client.GetAsync(Constants.GetKeysUrl);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorContent}");
        }

        Dictionary<int, string> keys = await response.Content.ReadFromJsonAsync<Dictionary<int, string>>();
        return keys;
    }
}
