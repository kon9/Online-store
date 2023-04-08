using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineStore.Library.Clients.IdentityServer;
using OnlineStore.Library.Clients.UserManagementService;
using OnlineStore.Library.Common.Models;
using OnlineStore.Library.Options;
using OnlineStore.Library.UserManagementService.Models;
using OnlineStore.Library.UserManagementService.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    public class AuthenticationServiceTest
    {
        private readonly IdentityServerClient _identityServerClient;
        private readonly UsersClient _usersClient;
        private readonly RolesClient _rolesClient;
        private readonly IdentityServerApiOptions _identityServerApiOptions;

        public AuthenticationServiceTest(IdentityServerClient identityServerClient, UsersClient usersClient,
            RolesClient rolesClient, IOptions<IdentityServerApiOptions> identityServerApiOptions)
        {
            _identityServerClient = identityServerClient;
            _usersClient = usersClient;
            _rolesClient = rolesClient;
            _identityServerApiOptions = identityServerApiOptions.Value;
        }

        public async Task<string> RunUserClientTest(string[] args)
        {
            var token = await _identityServerClient.GetApiToken(_identityServerApiOptions);
            _usersClient.HttpClient.SetBearerToken(token.AccessToken);
            var userName = "testUser";
            var roleName = "ShopClient4";
            var roleNames = new[] { roleName, "testRole2" };

            var addResult = await _usersClient.Add(new CreateUserRequest() { User = new ApplicationUser() { UserName = userName }, Password = "Password_1" });
            Console.WriteLine($"Add: {addResult}");
            Thread.Sleep(100);

            var changePasswordRequest = await _usersClient.ChangePassword(new UserPasswordChangeRequest() { UserName = userName, CurrentPassword = "Password_1", NewPassword = "Password_2" });
            Console.WriteLine($"Change password: {changePasswordRequest.Succeeded}");
            Thread.Sleep(100);

            var getOneRequest = await _usersClient.Get(userName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
            Thread.Sleep(100);

            var userToUpdate = getOneRequest.Payload;
            userToUpdate.DefaultAddress = new Address()
            {
                City = "Novosibirsk",
                Country = "Country rooooads take me hooome",
                PostalCode = "To the plaaace",
                AddressLine1 = "I beloong",
                AddressLine2 = "west virginia"
            };
            var updateResult = await _usersClient.Update(userToUpdate);
            Console.WriteLine($"Update: {updateResult.Succeeded}");
            Thread.Sleep(100);

            var addToRoleRequest = await _usersClient.AddToRole(new AddRemoveRoleRequest() { UserName = userName, RoleName = roleName });
            Console.WriteLine($"Add to role: {addToRoleRequest.Succeeded}");
            Thread.Sleep(100);

            var removeFromRoleRequest = await _usersClient.RemoveFromRole(new AddRemoveRoleRequest() { UserName = userName, RoleName = roleName });
            Console.WriteLine($"Remove from role: {removeFromRoleRequest.Succeeded}");
            Thread.Sleep(100);

            var addToRolesRequest = await _usersClient.AddToRoles(new AddRemoveRolesRequest() { UserName = userName, RoleNames = roleNames });
            Console.WriteLine($"Add to many roles: {addToRolesRequest.Succeeded}");
            Thread.Sleep(100);

            var removeFromRolesRequest = await _usersClient.RemoveFromRoles(new AddRemoveRolesRequest() { UserName = userName, RoleNames = roleNames });
            Console.WriteLine($"Remove from many roles: {removeFromRolesRequest.Succeeded}");

            getOneRequest = await _usersClient.Get(userName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
            Thread.Sleep(100);

            var deleteResult = await _usersClient.Remove(getOneRequest.Payload);
            Console.WriteLine($"Delete: {deleteResult.Succeeded}");
            Thread.Sleep(100);

            var getAllRequest = await _usersClient.GetAll();
            Console.WriteLine($"Get all: {getOneRequest.Code}");
            Thread.Sleep(100);

            return "ok";
        }

        public async Task<string> RunRolesClientTest(string[] args)
        {
            var token = await _identityServerClient.GetApiToken(_identityServerApiOptions);
            _rolesClient.HttpClient.SetBearerToken(token.AccessToken);
            var roleName = "testRoleName";

            var addResult = await _rolesClient.Add(new IdentityRole(roleName));
            Console.WriteLine($"Add: {addResult.Succeeded}");
            Thread.Sleep(100);

            var getOneRequest = await _rolesClient.Get(roleName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
            Thread.Sleep(100);

            var userToUpdate = getOneRequest.Payload;
            var updateResult = await _rolesClient.Update(userToUpdate);
            Console.WriteLine($"Update: {updateResult.Succeeded}");
            Thread.Sleep(100);

            getOneRequest = await _rolesClient.Get(roleName);
            Console.WriteLine($"Get one: {getOneRequest.Code}");
            Thread.Sleep(100);

            var deleteResult = await _rolesClient.Remove(getOneRequest.Payload);
            Console.WriteLine($"Delete: {deleteResult.Succeeded}");
            Thread.Sleep(100);

            var getAllRequest = await _rolesClient.GetAll();
            Console.WriteLine($"Get all: {getOneRequest.Code}");
            Thread.Sleep(100);

            return "Ok";
        }
    }
}