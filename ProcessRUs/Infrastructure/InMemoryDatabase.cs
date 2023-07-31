using Microsoft.AspNetCore.Identity;
using ProcessRUs.Helpers;
using ProcessRUs.Models;

namespace ProcessRUs.Infrastructure
{
    public static class InMemoryDatabase
    {

        public static void Seed(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
            {
                var adminRole = new IdentityRole(UserRoles.Admin);
                var result = roleManager.CreateAsync(adminRole).Result;
            }


            if (!roleManager.RoleExistsAsync(UserRoles.FrontOffice).Result)
            {
                var frontOfficeRole = new IdentityRole(UserRoles.FrontOffice);
                var result = roleManager.CreateAsync(frontOfficeRole).Result;
            }

            if (!roleManager.RoleExistsAsync(UserRoles.BackOffice).Result)
            {
                var backOfficeRole = new IdentityRole(UserRoles.BackOffice);
                var result = roleManager.CreateAsync(backOfficeRole).Result;
            }

            // Add test users 

            var user1 = new ApplicationUser
            {
                UserName = "AdminUser",
            };
            var result1 = userManager.CreateAsync(user1, "StrongPassword1@").Result;
            if (result1.Succeeded)
            {
                var roleResult = userManager.AddToRoleAsync(user1, UserRoles.Admin).Result;
            }



            var user2 = new ApplicationUser
            {
                UserName = "FrontOfficeUser",
            };
            var result2 = userManager.CreateAsync(user2, "StrongPassword1@").Result;
            if (result2.Succeeded)
            {
                var roleResult = userManager.AddToRoleAsync(user2, UserRoles.FrontOffice).Result;
            }



            var user3 = new ApplicationUser
            {
                UserName = "BackOfficeUser",
            };
            var result3 = userManager.CreateAsync(user3, "StrongPassword1@").Result;
            if (result3.Succeeded)
            {
                var roleResult = userManager.AddToRoleAsync(user3, UserRoles.BackOffice).Result;
            }
        }
    }
}
