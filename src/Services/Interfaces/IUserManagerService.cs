using BookVerse.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookVerse.Services.Interfaces 
{
    public interface IUserManagerService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string? id);
        Task<User> CreateUser([FromBody] User user);
        Task<User> UpdateUser(HttpRequest request);
        Task DeleteUser(string? id);
    }
}

