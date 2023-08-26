using OnlineStore.Library.IdentityServer;
using OnlineStore.Library.Options;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.IdentityServer
{
    public interface IIdentityServerClient
    {
        Task<Token> GetApiToken(IdentityServerApiOptions options);
    }
}