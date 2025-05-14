using ClipShare.Entities;
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
                foreach(var role in SD.Roles)
                {
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = role
                    });
                }
            }
        }
    }
}
