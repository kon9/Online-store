using Microsoft.AspNetCore.Mvc;
using OnlineStore.ApiService.Authorization;
using OnlineStore.Library.Clients;

namespace OnlineStore.ApiService.Controllers;

public class ControllerWithClientAuthorization<TClient> : ControllerBase
{
    protected readonly TClient Client;

    public ControllerWithClientAuthorization(TClient client, IClientAuthorization clientAuthorization)
    {
        Client = client;
        if (Client is IHttpClientContainer clientContainer)
        {
            clientAuthorization.Authorize(clientContainer).Wait();
        }
    }
}