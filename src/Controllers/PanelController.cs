using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAccount")]
    public class PanelController : Controller
    {
        [Route("{controller}/{action=Home}")]
        public IActionResult Home()
        {
            string userEmail = HttpContext.User.Identity.Name;

            var model = new User { Email = userEmail };
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserAccount");
            return RedirectToAction("Index", "Account");
        }
    }
}

