﻿using System;
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
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class UserHomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string userEmail = HttpContext.User.Identity.Name;

            var model = new User { Email = userEmail };
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserAuth"); 
            return RedirectToAction("Index", "UserAuth");
        }
    }
}

