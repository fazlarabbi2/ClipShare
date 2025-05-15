using ClipShare.Entities;
using ClipShare.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
