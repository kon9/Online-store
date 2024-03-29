﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineStore.Library.IdentityServer;
using OnlineStore.Library.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.IdentityServer
{
    public class IdentityServerClient : IIdentityServerClient
    {
        public IdentityServerClient(HttpClient client, IOptions<ServiceAddressOptions> options)
        {
            HttpClient = client;
            HttpClient.BaseAddress = new Uri(options.Value.IdentityServer);
        }
        public HttpClient HttpClient { get; }

        public async Task<Token> GetApiToken(IdentityServerApiOptions options)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", options.ClientId),
                new KeyValuePair<string, string>("client_secret", options.ClientSecret),
                new KeyValuePair<string, string>("scope", options.Scope),
                new KeyValuePair<string, string>("grant_type", options.GrantType)
            };

            var content = new FormUrlEncodedContent(keyValues);
            var response = await HttpClient.PostAsync("/connect/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine($"Content: {responseContent}");
                throw new InvalidOperationException("An error occurred while getting the API token.");
            }

            var token = JsonConvert.DeserializeObject<Token>(responseContent);//Bug here - token values is null.To fix change the property names in the Token class to match the JSON property names
            return token;
        }
    }
}
