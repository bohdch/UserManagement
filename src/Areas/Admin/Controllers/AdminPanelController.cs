using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserManagement.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class AdminPanelController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AdminAccount");
            return RedirectToAction("Index", "AdminAccount");
        }
    }
}
