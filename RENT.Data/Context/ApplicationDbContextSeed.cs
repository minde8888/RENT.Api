using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.Api.Configuration.Roles;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Roles;

namespace WTP.Data.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new ApplicationRole(Authorization.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Authorization.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Authorization.Roles.Manager.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Authorization.Roles.Employee.ToString()));
            //Seed Default User
            var defaultUser = new ApplicationUser { UserName = Authorization.default_username, Email = Authorization.default_email, EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, Authorization.default_password);
                await userManager.AddToRoleAsync(defaultUser, Authorization.default_role.ToString());
            }
        }
    }
}
