using BookVerse.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookVerse.Controllers.Api
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
