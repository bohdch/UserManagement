using UserManagement.Services;
using System.Data.SqlClient;
using System;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services.Interfaces 
{
    public interface IUserManagerService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string? id);
        Task<User> CreateUser(HttpRequest request);
        Task<User> UpdateUser(HttpRequest request);
        Task DeleteUser(string? id);
    }
}

