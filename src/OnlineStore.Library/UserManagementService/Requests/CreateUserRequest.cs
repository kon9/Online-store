using OnlineStore.Library.UserManagementService.Models;

namespace OnlineStore.Library.UserManagementService.Requests
{
    public class CreateUserRequest
    {
        public ApplicationUser User { get; set; }
        public string Password { get; set; }
    }
}