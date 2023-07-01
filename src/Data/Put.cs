using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;

internal static class Put
{
    public static async Task UpdateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbContext)
    {
        try
        {
            // Read user data from the request body
            var userData = await request.ReadFromJsonAsync<User>();

            if (userData != null)
            {
                User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

                if (user != null)
                {
                    user.FirstName = userData.FirstName;
                    user.LastName = userData.LastName;
                    user.Age = userData.Age;
                    user.Email = userData.Email;

                    await dbContext.SaveChangesAsync();

                    // Return updated user data
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    // User not found, return 404
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "No user found" });
                }
            }
            else
            {
                // Incorrect data received, return 400
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
}
