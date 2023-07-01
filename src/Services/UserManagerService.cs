using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Interfaces;


namespace UserManagement.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManagementDbContext _dbContext;

        public UserManagerService(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task GetAllUsers(HttpResponse response)
        {
            try
            {
                List<User> users = await _dbContext.Users.ToListAsync();
                await response.WriteAsJsonAsync(users);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                await response.WriteAsync("An error occurred while retrieving users.");
            }
        }

        public async Task GetUser(string? id, HttpResponse response)
        {
            try
            {
                User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user != null)
                {
                    await response.WriteAsJsonAsync(user);
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
                await response.WriteAsync("An error occurred while retrieving the user.");
            }
        }

        public async Task CreateUser(HttpRequest request, HttpResponse response)
        {
            try
            {
                var user = await request.ReadFromJsonAsync<User>();

                if (user != null)
                {
                    user.Id = Guid.NewGuid().ToString();
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();

                    response.StatusCode = 201;
                    await response.WriteAsJsonAsync(user);
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
                await response.WriteAsync("An error occurred while creating the user.");
            }
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
