using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
