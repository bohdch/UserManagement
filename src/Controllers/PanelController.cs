using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BookVerse.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BookVerse.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAccount")]
    public class PanelController : Controller
    {
        [Route("{controller}/{action=Library}")]
        public async Task<IActionResult> Library()
        {
            var model = await GetUserModelAsync();
            return View(model);
        }

        public async Task<IActionResult> Collections()
        {
            var model = await GetUserModelAsync();
            return View(model);
        }

        public async Task<IActionResult> CollectionsManager()
        {
            var model = await GetUserModelAsync();
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserAccount");
            return RedirectToAction("Index", "Account");
        }

        private async Task<User> GetUserModelAsync()
        {
            string userName = HttpContext.User.Identity.Name;
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return new User { Name = userName, Id = userId };
        }
    }
}

