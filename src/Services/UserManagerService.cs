﻿using System;
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

        public async Task CreateUser(HttpRequest request, HttpResponse response)
        {
            var user = await request.ReadFromJsonAsync<User>();

            user.Id = Guid.NewGuid().ToString();
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Return a 201 Created response with the new user data
            response.StatusCode = 201;
            await response.WriteAsJsonAsync(user);
        }

        public async Task UpdateUser(HttpRequest request, HttpResponse response)
        {
            try
            {
                var userData = await request.ReadFromJsonAsync<User>();

                if (userData != null)
                {
                    User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

                    if (user != null)
                    {
                        user.FirstName = userData.FirstName;
                        user.LastName = userData.LastName;
                        user.Age = userData.Age;
                        user.Email = userData.Email;

                        await _dbContext.SaveChangesAsync();

                        await response.WriteAsJsonAsync(user);
                    }
                    else
                    {
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "No user found" });
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    await response.WriteAsync("Invalid user data.");
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                await response.WriteAsync("An error occurred while updating the user.");
            }
        }

        public async Task DeleteUser(string? id, HttpResponse response)
        {
            try
            {
                User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();

                    response.StatusCode = 200;
                    await response.WriteAsJsonAsync(new { message = "User deleted successfully" });
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "No user found" });
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                await response.WriteAsync("An error occurred while deleting the user.");
            }
        }
    }
}
