using ClipShare.Entities;
using ClipShare.Utility;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
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

                var johnChannel = new Channel
                {
                    Name = "JohnChannel",
                    About = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    AppUserId = john.Id
                };

                context.Channel.Add(johnChannel);

                var peter = new AppUser
                {
                    Name = "Peter",
                    UserName = "peter",
                    Email = "peter@example.com"
                };

                await userManager.CreateAsync(peter, "Password123");
                await userManager.AddToRoleAsync(peter, SD.UserRole);

                var peterChannel = new Channel
                {
                    Name = "PeterChannel",
                    About = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    AppUserId = peter.Id
                };

                context.Channel.Add(peterChannel);

                var marry = new AppUser
                {
                    Name = "Marry",
                    UserName = "marry",
                    Email = "marry@example.com"
                };

                await userManager.CreateAsync(marry, "Password123");
                await userManager.AddToRoleAsync(marry, SD.UserRole);

                // adding new categories
                var animal = new Category { Name = "Animal" };
                var food = new Category { Name = "Food" };
                var game = new Category { Name = "Game" };
                var nature = new Category { Name = "Nature" };
                var news = new Category { Name = "News" };
                var sport = new Category { Name = "Sport" };

                context.Category.Add(animal);
                context.Category.Add(food);
                context.Category.Add(game);
                context.Category.Add(nature);
                context.Category.Add(news);
                context.Category.Add(sport);

                // adding videos and images into our Video Table
                var imageDirectory = new DirectoryInfo("Seed/Files/Thumbnails");
                var videoDirectory = new DirectoryInfo("Seed/Files/Videos");

                var description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).";

                FileInfo[] imageFiles = imageDirectory.GetFiles();
                FileInfo[] videoFiles = videoDirectory.GetFiles();

                for (int i = 0; i < 30; i++)
                {
                    var allNames = videoFiles[i].Name.Split("-");
                    var categoryName = allNames[0];
                    var titles = allNames[2].Split(".")[0];
                    var categoryId = await context.Category.Where(x => x.Name.ToLower() == categoryName).Select(x => x.Id).FirstOrDefaultAsync();
                }
            }
        }
    }
}