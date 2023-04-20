using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Clients.UserManagementService;
using OnlineStore.Library.Common.Models;
using OnlineStore.Library.Options;
using OnlineStore.Library.UserManagementService.Models;
using OnlineStore.Library.UserManagementService.Requests;
using System;
using System.Threading.Tasks;

namespace OnlineStore.ConsoleTestApp
{
    public class AuthenticationServiceTest
    {
        private readonly AspIdentityClient _identityServerClient;
        private readonly UsersClient _usersClient;
        private readonly RolesClient _rolesClient;
        private readonly AspIdentityApiOptions _aspIdentityApiOptions;

        public AuthenticationServiceTest(AspIdentityClient identityServerClient, UsersClient usersClient,
            RolesClient rolesClient, IOptions<AspIdentityApiOptions> aspIdentityApiOptions)
        {
            _identityServerClient = identityServerClient;
            _usersClient = usersClient;
            _rolesClient = rolesClient;
            _aspIdentityApiOptions = aspIdentityApiOptions.Value;
        }
        public async Task<string> RunUserClientTest(string userName)
        {
            var token = await _identityServerClient.GetApiToken(_aspIdentityApiOptions);
            _usersClient.HttpClient.SetBearerToken(token.AccessToken);

            await AddUserTest(userName);
            await ChangePasswordTest(userName);
            await GetUserTest(userName);
            await UpdateUserTest(userName);
            await AddUserToRoleTest(userName);
            await RemoveUserFromRoleTest(userName);
            await AddUserToRolesTest(userName);
            await RemoveUserFromRolesTest(userName);
            await GetUserTest(userName);
            await DeleteUserTest(userName);
            await GetAllUsersTest();
            return "Tests completed";
        }

        public async Task<string> RunRolesClientTest(string roleName)
        {
            var token = await _identityServerClient.GetApiToken(_aspIdentityApiOptions);
            _rolesClient.HttpClient.SetBearerToken(token.AccessToken);

            await AddRoleTest(roleName);
            await GetRoleTest(roleName);
            await UpdateRoleTest(roleName);
            await GetRoleTest(roleName);
            await DeleteRoleTest(roleName);
            await GetAllRolesTest();
            return "Tests completed";
        }

        private async Task AddUserTest(string userName)
        {
            var addResult = await _usersClient.Add(new CreateUserRequest() { User = new ApplicationUser() { UserName = userName }, Password = "Password_1" });
            Console.WriteLine($"Add: {addResult}");
        }

        private async Task ChangePasswordTest(string userName)
        {
            var changePasswordRequest = await _usersClient.ChangePassword(new UserPasswordChangeRequest() { UserName = userName, CurrentPassword = "Password_1", NewPassword = "Password_2" });
            Console.WriteLine($"Change password: {changePasswordRequest.Succeeded}");
        }

        private async Task GetUserTest(string userName)
        {
            var getOneRequest = await _usersClient.Get(userName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
        }

        private async Task UpdateUserTest(string userName)
        {
            var getOneRequest = await _usersClient.Get(userName);
            var userToUpdate = getOneRequest.Payload;
            userToUpdate.DefaultAddress = new Address()//todo:Address not deleting from db after tests bug
            {
                City = "Novosibirsk",
                Country = "Country rooooads take me hooome",
                PostalCode = "To the plaaace",
                AddressLine1 = "I beloong",
                AddressLine2 = "west virginia"
            };
            var updateResult = await _usersClient.Update(userToUpdate);
            Console.WriteLine($"Update: {updateResult.Succeeded}");
        }
        private async Task AddUserToRoleTest(string userName)
        {
            var roleName = "ShopClient4";
            var addToRoleRequest = await _usersClient.AddToRole(new AddRemoveRoleRequest() { UserName = userName, RoleName = roleName });
            Console.WriteLine($"Add to role: {addToRoleRequest.Succeeded}");
        }

        private async Task RemoveUserFromRoleTest(string userName)
        {
            var roleName = "ShopClient4";
            var removeFromRoleRequest = await _usersClient.RemoveFromRole(new AddRemoveRoleRequest() { UserName = userName, RoleName = roleName });
            Console.WriteLine($"Remove from role: {removeFromRoleRequest.Succeeded}");
        }

        private async Task AddUserToRolesTest(string userName)
        {
            var roleNames = new[] { "ShopClient4", "testRole2" };
            var addToRolesRequest = await _usersClient.AddToRoles(new AddRemoveRolesRequest() { UserName = userName, RoleNames = roleNames });
            Console.WriteLine($"Add to many roles: {addToRolesRequest.Succeeded}");
        }

        private async Task RemoveUserFromRolesTest(string userName)
        {
            var roleNames = new[] { "ShopClient4", "testRole2" };
            var removeFromRolesRequest = await _usersClient.RemoveFromRoles(new AddRemoveRolesRequest() { UserName = userName, RoleNames = roleNames });
            Console.WriteLine($"Remove from many roles: {removeFromRolesRequest.Succeeded}");
        }

        private async Task DeleteUserTest(string userName)
        {
            var getOneRequest = await _usersClient.Get(userName);
            var deleteResult = await _usersClient.Remove(getOneRequest.Payload);
            Console.WriteLine($"Delete: {deleteResult.Succeeded}");
        }

        private async Task GetAllUsersTest()
        {
            var getAllRequest = await _usersClient.GetAll();
            Console.WriteLine($"Get all: {getAllRequest.Code}");
        }

        private async Task AddRoleTest(string roleName)
        {
            var addResult = await _rolesClient.Add(new IdentityRole(roleName));
            Console.WriteLine($"Add: {addResult.Succeeded}");
        }

        private async Task GetRoleTest(string roleName)
        {
            var getOneRequest = await _rolesClient.Get(roleName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
        }

        private async Task UpdateRoleTest(string roleName)
        {
            var getOneRequest = await _rolesClient.Get(roleName);
            var roleToUpdate = getOneRequest.Payload;
            var updateResult = await _rolesClient.Update(roleToUpdate);
            Console.WriteLine($"Update: {updateResult.Succeeded}");
        }

        private async Task DeleteRoleTest(string roleName)
        {
            var getOneRequest = await _rolesClient.Get(roleName);
            var deleteResult = await _rolesClient.Remove(getOneRequest.Payload);
            Console.WriteLine($"Delete: {deleteResult.Succeeded}");
        }

        private async Task GetAllRolesTest()
        {
            var getAllRequest = await _rolesClient.GetAll();
            Console.WriteLine($"Get all: {getAllRequest.Code}");
        }
    }
}