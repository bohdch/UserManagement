using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;

internal static class Post
{
    public static async Task CreateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbContext)
    {
        try
        {
            // Deserialize the user data from the request body
            var user = await request.ReadFromJsonAsync<User>();

            if (user != null)
            {
                user.Id = Guid.NewGuid().ToString();
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                // Return a 201 Created response with the new user data
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
}
