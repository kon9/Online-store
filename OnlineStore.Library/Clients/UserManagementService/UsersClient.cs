using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineStore.Library.Constants;
using OnlineStore.Library.Options;
using OnlineStore.Library.UserManagementService.Models;
using OnlineStore.Library.UserManagementService.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.UserManagementService
{
    public class UsersClient : UserManagementBaseClient, IUserClient
    {
        public UsersClient(HttpClient client, IOptions<ServiceAddressOptions> options) : base(client, options)
        {
        }
        public async Task<IdentityResult> Add(CreateUserRequest request)
        {
            return await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Add}");
        }

        public async Task<IdentityResult> AddToRole(AddRemoveRoleRequest request)
        {
            return await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.AddToRole}");
        }

        public async Task<IdentityResult> AddToRoles(AddRemoveRolesRequest request)
        {
            return await SendPostRequest(request,
                $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.AddToRoles}");
        }

        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request)
        {
            return await SendPostRequest(request,
                $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.ChangePassword}");
        }

        public async Task<UserManagementServiceResponse<ApplicationUser>> Get(string name)
        {
            return await SendGetRequest<ApplicationUser>($"/{UsersControllerRoutes.ControllerName}?name={name}");
        }

        public async Task<UserManagementServiceResponse<IEnumerable<ApplicationUser>>> GetAll()
        {
            return await SendGetRequest<IEnumerable<ApplicationUser>>($"/{UsersControllerRoutes.ControllerName}/{RepoActions.GetAll}");
        }

        public async Task<IdentityResult> Remove(ApplicationUser user)
        {
            return await SendPostRequest(user, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Remove}");
        }

        public async Task<IdentityResult> RemoveFromRole(AddRemoveRoleRequest request)
        {
            return await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.RemoveFromRole}");
        }

        public async Task<IdentityResult> RemoveFromRoles(AddRemoveRolesRequest request)
        {
            return await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.RemoveFromRoles}");
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            return await SendPostRequest(user, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Update}");
        }
    }
}