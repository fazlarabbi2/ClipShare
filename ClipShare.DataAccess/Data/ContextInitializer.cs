using ClipShare.Entities;
using ClipShare.Utility;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClipShare.DataAccess.Data
{
    public static class ContextInitializer
    {
        // Pseudocode plan:
        // 1. Remove the check for !userManager.Users.Any() to always ensure users are created if missing.
        // 2. For each user (admin, john, mary), check if a user with the same UserName exists.
        // 3. If not, create the user and assign roles as needed.

        public static async Task InitializeAsync(Context context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            if (context.Database.GetAppliedMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }

            // Ensure all roles in SD.Roles are created
            foreach (var role in SD.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            // Ensure admin user exists
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var admin = new AppUser
                {
                    Name = "admin",
                    Email = "admin@example.com",
                    UserName = "admin"
                };
                await userManager.CreateAsync(admin, "Password123");
                await userManager.AddToRolesAsync(admin, new[] { SD.AdminRole, SD.UserRole, SD.ModeratorRole });
            }

            // Ensure john user exists
            var johnUser = await userManager.FindByNameAsync("john");
            if (johnUser == null)
            {
                var john = new AppUser
                {
                    Name = "john",
                    Email = "john@example.com",
                    UserName = "john"
                };
                await userManager.CreateAsync(john, "Password123");
                await userManager.AddToRoleAsync(john, SD.UserRole);
            }

            // Ensure mary user exists
            var maryUser = await userManager.FindByNameAsync("mary");
            if (maryUser == null)
            {
                var mary = new AppUser
                {
                    Name = "mary",
                    Email = "mary@example.com",
                    UserName = "mary"
                };
                await userManager.CreateAsync(mary, "Password123");
                await userManager.AddToRoleAsync(mary, SD.ModeratorRole);
            }
        }
    }
}
