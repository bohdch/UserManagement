using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookVerse.Data;
using BookVerse.Models;
using BookVerse.Services.Interfaces;

namespace BookVerse.Services
{
    public class AccountService : IAccountService
    {
        private readonly BookVerseDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(BookVerseDbContext dbContext, IHttpContextAccessor httpContextAccessor)
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

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserAccount");

            await _httpContextAccessor.HttpContext.SignInAsync("UserAccount", new ClaimsPrincipal(claimsIdentity));

            return new RedirectToActionResult("Library", "Panel", null);
        }
    }
}
