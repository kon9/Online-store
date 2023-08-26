using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.ApiService.Authorization;
using OnlineStore.Library.Clients.UserManagementService;
using OnlineStore.Library.Constants;
using OnlineStore.Library.UserManagementService.Models;
using OnlineStore.Library.UserManagementService.Requests;

namespace OnlineStore.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerWithClientAuthorization<IUserClient>
{
    public UsersController(IUserClient client, IClientAuthorization clientAuthorization) : base(client,
        clientAuthorization)
    {
    }

    [HttpPost(RepoActions.Add)]
    public Task<IdentityResult> Add(CreateUserRequest request)
    {
        return Client.Add(request);
    }

    [HttpPost(RepoActions.Update)]
    public Task<IdentityResult> Update(ApplicationUser request)
    {
        return Client.Update(request);
    }

    [HttpPost(RepoActions.Remove)]
    public Task<IdentityResult> Remove(ApplicationUser request)
    {
        return Client.Remove(request);
    }

    [HttpGet]
    public async Task<ApplicationUser> Get(string name)
    {
        var result = await Client.Get(name);
        return result.Payload;
    }

    [HttpGet(RepoActions.GetAll)]
    public async Task<IEnumerable<ApplicationUser>> Get()
    {
        var result = await Client.GetAll();
        return result.Payload;
    }

    [HttpPost(UsersControllerRoutes.ChangePassword)]
    public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request)
    {
        var result = await Client.ChangePassword(request);
        return result;
    }

    [HttpPost(UsersControllerRoutes.AddToRole)]
    public async Task<IdentityResult> AddToRole(AddRemoveRoleRequest request)
    {
        var result = await Client.AddToRole(request);
        return result;
    }

    [HttpPost(UsersControllerRoutes.AddToRoles)]
    public async Task<IdentityResult> AddToRoles(AddRemoveRolesRequest request)
    {
        var result = await Client.AddToRoles(request);
        return result;
    }

    [HttpPost(UsersControllerRoutes.RemoveFromRole)]
    public async Task<IdentityResult> RemoveFromRole(AddRemoveRoleRequest request)
    {
        var result = await Client.RemoveFromRole(request);
        return result;
    }

    [HttpPost(UsersControllerRoutes.RemoveFromRoles)]
    public async Task<IdentityResult> RemoveFromRoles(AddRemoveRolesRequest request)
    {
        var result = await Client.AddToRoles(request);
        return result;
    }
}


