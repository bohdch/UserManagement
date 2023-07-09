using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;
using UserManagement.Models;
using UserManagement.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using UserManagement.Services.Interfaces;
using UserManagement.Controllers.Api;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IAccountService _accountService;
        private readonly IUserApiController _userApiController;

        public AccountController(UserManagementDbContext dbContext, IAccountService userAuthService, IUserApiController userApiController)
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
                return View("Index");
            }
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp(string name, string email, string password)
        {
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