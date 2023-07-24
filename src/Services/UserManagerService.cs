using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BookVerse.Data;
using BookVerse.Models;
using BookVerse.Services.Interfaces;
using BookVerse.Controllers;

namespace BookVerse.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly BookVerseDbContext _dbContext;

        public UserManagerService(BookVerseDbContext dbContext)
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

        public async Task<User> CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.Now;
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUser(HttpRequest request)
        {
            var userData = await request.ReadFromJsonAsync<User>();

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

            user.Name = userData.Name;
            user.Email = userData.Email;
            user.Phone = userData.Phone;
            user.CreatedAt = userData.CreatedAt;
            user.Verified = userData.Verified;

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
