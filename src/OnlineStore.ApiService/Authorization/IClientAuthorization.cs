using OnlineStore.Library.Clients;

namespace OnlineStore.ApiService.Authorization;

public interface IClientAuthorization
{
    Task Authorize(IHttpClientContainer client);
}