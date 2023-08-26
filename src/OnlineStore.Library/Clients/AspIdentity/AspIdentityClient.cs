using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineStore.Library.IdentityServer;
using OnlineStore.Library.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.AspIdentity;

public class AspIdentityClient : IAspIdentityClient
{
    public AspIdentityClient(HttpClient client, IOptions<ServiceAddressOptions> options)
    {
        HttpClient = client;
        HttpClient.BaseAddress = new Uri(options.Value.AspIdentityServer);
    }
    public HttpClient HttpClient { get; }

    public async Task<Token> GetApiToken(AspIdentityApiOptions options)
    {
        var jsonContent = $"{{\"Email\":\"{options.UserName}\",\"Password\":\"{options.Password}\"}}";
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync("api/auth/login", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"Content: {responseContent}");
            throw new InvalidOperationException("An error occurred while getting the API token.");
        }

        var token = JsonConvert.DeserializeObject<Token>(responseContent);
        return token;
    }
}