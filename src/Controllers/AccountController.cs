using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;
using BookVerse.Models;
using BookVerse.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using BookVerse.Services.Interfaces;
using BookVerse.Controllers.Api;

namespace BookVerse.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookVerseDbContext _dbContext;
        private readonly IAccountService _accountService;
        private readonly IUserController _userApiController;

        public AccountController(BookVerseDbContext dbContext, IAccountService userAuthService, IUserController userApiController)
        {
            _dbContext = dbContext;
            _accountService = userAuthService;
            _userApiController = userApiController;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/signin")]
        public async Task<IActionResult> SignIn()
        {
            string email = Request.Form["email"];
            string password = Request.Form["password"];

            try
            {
                return await _accountService.SignIn(email, password);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["FormType"] = "sign-in";
                return View("Index");
            }
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp(string name, string email, string password)
        {
            // Check if the email is already taken
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "Email already exists. Please choose a different email");
                ViewData["FormType"] = "sign-up";
                ViewData["OpenSignupForm"] = true; // Open the signup form

                return View("Index");
            }

            // Check if the password is already taken
            if (_dbContext.Users.Any(u => u.Password == password))
            {
                ModelState.AddModelError("", "Password is already taken. Please choose a different password");
                ViewData["FormType"] = "sign-up";
                ViewData["OpenSignupForm"] = true; // Open the signup form

                return View("Index");
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                Password = password,
            };

            await _userApiController.CreateUser(newUser);

            // Sign in to the user page after user registration
            return await _accountService.SignIn(email, password);
        }
    }
}