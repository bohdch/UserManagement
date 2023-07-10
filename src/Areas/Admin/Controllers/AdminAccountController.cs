using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        [Route("/Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/admin/signin")]
        public async Task<IActionResult> SignIn()
        {
            string name = Request.Form["name"];
            string password = Request.Form["password"];

            Admin _admin = new Admin();

            // Perform validation of admin credentials
            if (name == _admin.Name && password == _admin.Password)
            {
                var claims = new[]
                 {
                    new Claim(ClaimTypes.Name, name),
                };

                var identity = new ClaimsIdentity(claims, "AdminAccount");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("AdminAccount", principal);

                return Redirect("/Admin/AdminPanel");

            }
            else
            {
                return RedirectToAction("Index", "AdminAccount");
            }
        }
    }
}
