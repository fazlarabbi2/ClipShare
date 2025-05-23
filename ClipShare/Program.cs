using ClipShare.DataAccess.Data;
using ClipShare.Entities;
using ClipShare.Extensions;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;

namespace ClipShare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.AddApplicationServices();
            builder.AddAuthenticationServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            InitializeContextAsync();
            app.Run();


            async Task InitializeContextAsync()
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;

                try
                {
                    var context = scope.ServiceProvider.GetService<Context>();
                    var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
                    var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();


                    await ContextInitializer.InitializeAsync(context, roleManager, userManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while migrating database");
                }
            }
        }
    }
}
