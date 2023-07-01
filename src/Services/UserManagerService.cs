using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Interfaces;
using UserManagement.Controllers;

namespace UserManagement.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManagementDbContext _dbContext;

        public UserManagerService(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> users = await _dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(string? id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> CreateUser(HttpRequest request)
        {
            var user = await request.ReadFromJsonAsync<User>();

            user.Id = Guid.NewGuid().ToString();
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUser(HttpRequest request)
        {
            var userData = await request.ReadFromJsonAsync<User>();

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.Age = userData.Age;
            user.Email = userData.Email;

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(string? id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
