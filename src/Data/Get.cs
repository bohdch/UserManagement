using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services;
using UserManagement.Data;
using UserManagement.Models;

internal static class Get
{
    public static async Task GetAllUsers(HttpResponse response, UserManagementDbContext dbContext)
    {
        try
        {
            List<User> users = await dbContext.Users.ToListAsync();
            await response.WriteAsJsonAsync(users);
        }

        catch (Exception ex)
        {
            // Handle the exception here, you can log the error or return an appropriate response
            response.StatusCode = 500;
            await response.WriteAsync("An error occurred while retrieving users.");
        }
    }

    public static async Task GetUser(string? id, HttpResponse response, UserManagementDbContext dbContext)
    {
        try
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

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
            // Handle the exception here, you can log the error or return an appropriate response
            response.StatusCode = 500;
            await response.WriteAsync("An error occurred while retrieving the user.");
        }
    }
}
