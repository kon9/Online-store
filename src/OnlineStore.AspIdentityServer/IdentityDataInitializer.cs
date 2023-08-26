using Microsoft.AspNetCore.Identity;
using OnlineStore.Library.UserManagementService.Models;

namespace OnlineStore.AspIdentityServer;

public class IdentityDataInitializer
{
    public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.FindByNameAsync("admin") == null)
        {
            var user = new ApplicationUser { UserName = "admin", Email = "admin@example.com" };
            var result = await userManager.CreateAsync(user, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var role = new IdentityRole { Name = "Admin" };
            await roleManager.CreateAsync(role);
        }
    }
}