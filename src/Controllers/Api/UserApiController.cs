using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Models;

namespace UserManagement.Controllers.Api
{
    [Route("/api/users")]
    [ApiController]
    public class UserApiController : ControllerBase, IUserApiController
    {
        private readonly IUserManagerService _userManagerService;

        public UserApiController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagerService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string? id)
        {
            var user = await _userManagerService.GetUser(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var createdUser = await _userManagerService.CreateUser(user);
            return Ok(createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser()
        {
            var user = await _userManagerService.UpdateUser(Request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string? id)
        {
            await _userManagerService.DeleteUser(id);
            return NoContent();
        }
    }
}
