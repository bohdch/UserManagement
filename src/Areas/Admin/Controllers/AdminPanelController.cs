using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
