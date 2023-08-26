using Microsoft.AspNetCore.Identity;
using OnlineStore.Library.UserManagementService.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients.UserManagementService
{
    public interface IRolesClient
    {
        Task<IdentityResult> Add(IdentityRole role);
        Task<UserManagementServiceResponse<IdentityRole>> Get(string name);
        Task<UserManagementServiceResponse<IEnumerable<IdentityRole>>> GetAll();
        Task<IdentityResult> Remove(IdentityRole role);
        Task<IdentityResult> Update(IdentityRole role);
    }
}