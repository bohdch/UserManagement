using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;

internal static class Delete
{
    public static async Task DeleteUser(string? id, HttpResponse response, UserManagementDbContext dbContext)
    {
        try
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();

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
