using UserManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Services.Interfaces.Api
{
    public interface IUserApiController
    {
        Task<IActionResult> GetAllUsers();
        Task<IActionResult> GetUser(string? id);
        Task<IActionResult> CreateUser([FromBody] User user);
        Task<IActionResult> UpdateUser();
        Task<IActionResult> DeleteUser(string? id);
    }
}
