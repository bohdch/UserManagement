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
        Task CreateUser(HttpRequest request, HttpResponse response);
        Task UpdateUser(HttpRequest request, HttpResponse response);
        Task DeleteUser(string? id, HttpResponse response);
    }
}

