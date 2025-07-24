using ClipShare.Entities;
using ClipShare.Utility;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Seed
{
    public static class ContextInitializer
    {
        public static async Task InitializerAsync(Context context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (context.Database.GetPendingMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }

            if (!roleManager.Roles.Any())
            {
                foreach (var role in SD.Roles)
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            if (!userManager.Users.Any())
            {
                var admin = new AppUser
                {
                    Name = "Admin",
                    UserName = "admin",
                    Email = "admin@example.com"
                };

                await userManager.CreateAsync(admin, "Password123");
                await userManager.AddToRolesAsync(admin, [SD.AdminRole, SD.UserRole, SD.ModeratorRole]);

                var john = new AppUser
                {
                    Name = "John",
                    UserName = "john",
                    Email = "john@example.com"
                };

                await userManager.CreateAsync(john, "Password123");
                await userManager.AddToRoleAsync(john, SD.UserRole);

                var peter = new AppUser
                {
                    Name = "Peter",
                    UserName = "peter",
                    Email = "peter@example.com"
                };

                await userManager.CreateAsync(john, "Password123");
                await userManager.AddToRoleAsync(john, SD.UserRole);

                var marry = new AppUser
                {
                    Name = "Marry",
                    UserName = "marry",
                    Email = "marry@example.com"
                };

                await userManager.CreateAsync(marry, "Password123");
                await userManager.AddToRoleAsync(marry, SD.UserRole);



            }
        }
    }
}