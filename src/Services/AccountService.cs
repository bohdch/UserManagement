using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManagementDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> SignIn(string email, string password)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user is null)
            {
                throw new InvalidOperationException("Invalid email or password, try again");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserAccount");

            await _httpContextAccessor.HttpContext.SignInAsync("UserAccount", new ClaimsPrincipal(claimsIdentity));

            _httpContextAccessor.HttpContext.Response.Cookies.Append("name", user.Email);

            return new RedirectToActionResult("Home", "Panel", null);
        }
    }
}
