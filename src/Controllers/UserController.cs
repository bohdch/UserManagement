using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
