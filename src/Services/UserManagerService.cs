using System;
using UserManagement.Services.Interfaces;
using UserManagement.Data;
using UserManagement.Models;
using System.Linq;

namespace UserManagement.Services
{
    public class UserManagerService : IUserManagerService
    {
        public async Task GetAllUsers(HttpResponse response, UserManagementDbContext dbService)
        {
            List<User> users = dbService.Users.ToList();
            await response.WriteAsJsonAsync(users);
        }

        public Task GetUser(string? id, HttpResponse response, UserManagementDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        public Task CreateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbService)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbService)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string? id, HttpResponse response, UserManagementDbContext dbService)
        {
            throw new NotImplementedException();
        }
    }
}
