using ClipShare.Entities;
using ClipShare.Utility;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClipShare.DataAccess.Data
{
    public static class ContextInitializer
    {
        public static async Task InitializeAsync(Context context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            if (context.Database.GetAppliedMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }

            if (!roleManager.Roles.Any())
            {
                foreach (var role in SD.Roles)
                {
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = role
                    });
                }
            }

            if (!userManager.Users.Any())
            {
                var admin = new AppUser
                {
                    Name = "admin",
                    Email = "admin@example.com",
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "Password123");
                await userManager.AddToRolesAsync(admin, [SD.AdminRole, SD.UserRole, SD.ModeratorRole]);


                if (!userManager.Users.Any())
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
                
                if (!userManager.Users.Any())
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
}
