using Microsoft.AspNetCore.Mvc;

namespace ClipShare.Controllers
{
    public class MemberController : CoreController
    {
        public IActionResult Channel(int id)
        {
            return View();
        }
    }
}
