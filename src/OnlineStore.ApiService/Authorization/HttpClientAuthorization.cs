using IdentityModel.Client;
using Microsoft.Extensions.Options;
using OnlineStore.Library.Clients;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Options;

namespace OnlineStore.ApiService.Authorization;

public class HttpClientAuthorization : IClientAuthorization
{
    private readonly IAspIdentityClient _aspIdentityClient;
    private readonly AspIdentityApiOptions _aspIdentityApiOptions;

    public HttpClientAuthorization(IAspIdentityClient client, IOptions<AspIdentityApiOptions> opt)
    {
        _aspIdentityClient = client;
        _aspIdentityApiOptions = opt.Value;
    }
    public async Task Authorize(IHttpClientContainer client)
    {
        var token = await _aspIdentityClient.GetApiToken(_aspIdentityApiOptions);
        client.HttpClient.SetBearerToken(token.AccessToken);
    }
}