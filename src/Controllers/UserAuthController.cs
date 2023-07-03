using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using UserManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserManagement.Controllers
{
    // Class that sends a page for user authentication
    public class UserAuthController : Controller
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IUserAuthService _userAuthService;

        public UserAuthController(UserManagementDbContext dbContext, IUserAuthService userAuthService)
        {
            _dbContext = dbContext;
            _userAuthService = userAuthService;
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
                return await _userAuthService.SignIn(email, password);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index");
            }
        }

        [HttpPost("/signup")]
        public IActionResult SignUp()
        {
            return _userAuthService.SignUp();
        }
    }
}
