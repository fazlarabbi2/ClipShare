using ClipShare.Entities;
using ClipShare.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClipShare.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : Controller
    {
        public IActionResult Login(string? returnUrl = null)
        {
            var loginVm = new Login_vm()
            {
                ReturnUrl = returnUrl
            };
            return View(loginVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login_vm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ReturnUrl = model.ReturnUrl??Url.Content("~/");

            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user = await userManager.FindByEmailAsync(model.UserName);
            }

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password. Please try again.");

                return View(model);
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                await HandleSignInUserAsync(user);
                return LocalRedirect(model.ReturnUrl);
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password. Please try again.");
                return View(model);
            }
        }

        #region Private Methods
        private async Task HandleSignInUserAsync(AppUser user)
        {
            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            var roles = await userManager.GetRolesAsync(user);
            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var principal = new ClaimsPrincipal(claimsIdentity);

            // Using this method to assign identityClaims into Usre.Identity and sign the user in using build in dotnet identity 
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        #endregion
    }
}
