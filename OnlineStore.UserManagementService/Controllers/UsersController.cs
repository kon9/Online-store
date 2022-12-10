using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.UserManagementService.Models;
using System.Threading.Tasks;
using OnlineStore.Library.UserManagementService.Requests;

namespace OnlineStore.UserManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes  = "Bearer")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("add")]
        public Task<IdentityResult> Add(CreateUserRequest request)
        {
            var result = _userManager.CreateAsync(request.User,request.Password);
            return result;
        }
    }
}
