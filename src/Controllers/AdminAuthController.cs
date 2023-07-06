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
	public class AdminAuthController : Controller
    {
        [Route("/admin")]
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
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, "AdminAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("AdminAuth", principal); 

                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Index", "AdminAuth");
            }
        }
    }
}
