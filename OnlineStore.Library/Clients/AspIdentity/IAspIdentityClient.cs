using System.Threading.Tasks;
using OnlineStore.Library.IdentityServer;
using OnlineStore.Library.Options;

namespace OnlineStore.Library.Clients.AspIdentity;

public interface IAspIdentityClient
{
    Task<Token> GetApiToken(AspIdentityApiOptions options);
}