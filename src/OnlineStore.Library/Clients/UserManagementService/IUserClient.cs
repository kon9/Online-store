using Microsoft.AspNetCore.Identity;
using OnlineStore.Library.UserManagementService.Models;
using OnlineStore.Library.UserManagementService.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.UserManagementService
{
    public interface IUserClient
    {
        Task<IdentityResult> Add(CreateUserRequest request);
        Task<IdentityResult> Update(ApplicationUser user);
        Task<IdentityResult> Remove(ApplicationUser user);
        Task<UserManagementServiceResponse<ApplicationUser>> Get(string name);
        Task<UserManagementServiceResponse<IEnumerable<ApplicationUser>>> GetAll();
        Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request);
        Task<IdentityResult> AddToRole(AddRemoveRoleRequest request);
        Task<IdentityResult> AddToRoles(AddRemoveRolesRequest request);
        Task<IdentityResult> RemoveFromRole(AddRemoveRoleRequest request);
        Task<IdentityResult> RemoveFromRoles(AddRemoveRolesRequest request);
    }
}