using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Interfaces;
using System.Threading.Tasks; 

namespace UserManagement.Controllers.Api
{
	public class UserApiController : ControllerBase
    {
		private readonly IUserManagerService _userManagerService;

		public UserApiController(IUserManagerService userManagerService)
		{
			_userManagerService = userManagerService;
		}

        [HttpGet]
        [Route("/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagerService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("/users/{id}")]
        public async Task<IActionResult> GetUser(string? id)
        {
            var user = await _userManagerService.GetUser(id);
            return Ok(user);
        }

        // POST: /api/users
        [HttpPost]
        [Route("/users")]
        public async Task CreateUser()
        {
            await _userManagerService.CreateUser(Request, Response);
        }
    }
}
