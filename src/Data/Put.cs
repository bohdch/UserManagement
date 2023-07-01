using System.Data.SqlClient;
using UserManagement.Services.Data;

// Define a static method called UpdateUser that handles updating an existing user
internal static class Put {
    // Handles updating an existing user by Id
    public static async Task UpdateUser(HttpRequest request, HttpResponse response, DatabaseService dbService) {
        try {
            // Read user data from the request body
            var userData = await request.ReadFromJsonAsync<User>();

            if (userData != null) {
                using (var connection = dbService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("spUpdateUser", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        // Set the parameter values
                        command.Parameters.AddWithValue("@FirstName", userData.FirstName);
                        command.Parameters.AddWithValue("@LastName", userData.LastName);
                        command.Parameters.AddWithValue("@Age", userData.Age);
                        command.Parameters.AddWithValue("@Email", userData.Email);
                        command.Parameters.AddWithValue("@Id", userData.Id);

                        await command.ExecuteNonQueryAsync();
                    }

                    // Return updated user data
                    await response.WriteAsJsonAsync(userData);
                }
            } else {
                // Incorrect data received, return 404
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "No user found" });
            }
        } catch {
            // Error occurred while processing request, return 404
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "Incorrect data" });
        }
    }
}

