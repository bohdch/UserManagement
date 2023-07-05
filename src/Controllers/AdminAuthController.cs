using System;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
	public class AdminAuthController : Controller
    {
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
