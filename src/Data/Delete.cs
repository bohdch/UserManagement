using System.Data.SqlClient;
using UserManagement.Services.Data;

// This class contains a static method for deleting a user
internal static class Delete
{
    // Method deletes an existing user by Id
    public static async Task DeleteUser(string? id, HttpResponse response, DatabaseService dbService)
    {
        try
        {
            using (var connection = dbService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("spDeleteUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        // User deleted successfully
                        response.StatusCode = 200;
                        await response.WriteAsJsonAsync(new { message = "User deleted successfully" });
                    }
                    else
                    {
                        // User not found, return 404
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "No user found" });
                    }
                }
            }
        }
        catch
        {
            // Error occurred while processing request, return 404
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "Incorrect data" });
        }
    }
}
