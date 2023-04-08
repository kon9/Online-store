using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineStore.Library.Constants;
using OnlineStore.Library.Options;
using OnlineStore.Library.UserManagementService.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.UserManagementService
{
    public class RolesClient : UserManagementBaseClient, IRolesClient
    {
        public RolesClient(HttpClient client, IOptions<ServiceAddressOptions> options) : base(client, options)
        {
        }

        public async Task<IdentityResult> Add(IdentityRole role)
        {
            return await SendPostRequest(role, $"/{RolesControllerRoutes.ControllerName}/{RepoActions.Add}");
        }

        public async Task<UserManagementServiceResponse<IdentityRole>> Get(string name)
        {
            return await SendGetRequest<IdentityRole>($"/{RolesControllerRoutes.ControllerName}?name={name}");
        }

        public async Task<UserManagementServiceResponse<IEnumerable<IdentityRole>>> GetAll()
        {
            return await SendGetRequest<IEnumerable<IdentityRole>>(
                $"/{RolesControllerRoutes.ControllerName}/{RepoActions.GetAll}");
        }

        public async Task<IdentityResult> Remove(IdentityRole role)
        {
            return await SendPostRequest(role, $"/{RolesControllerRoutes.ControllerName}/{RepoActions.Remove}");
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            return await SendPostRequest(role, $"/{RolesControllerRoutes.ControllerName}/{RepoActions.Update}");
        }
    }
}