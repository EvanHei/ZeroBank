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
        Credentials credentials = new(username, password);

        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminLoginUrl, credentials);
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
        Credentials credentials = new(username, password);

        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminCreateUrl, credentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }

    public async Task AdminDelete(string username, string password)
    {
        Credentials credentials = new(username, password);

        HttpResponseMessage response = await client.PostAsJsonAsync(Constants.AdminDeleteUrl, credentials);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Server error (HTTP {response.StatusCode}): {errorMessage}");
        }
    }
}
