using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string userEmail = Request.Cookies["name"];

            var model = new User { Email = userEmail };
            return View(model);
        }
    }
}

