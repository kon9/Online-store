using System.Net.Http;

namespace OnlineStore.Library.Clients;

public interface IHttpClientContainer
{
    public HttpClient HttpClient { get; }
}
