using System.Data.SqlClient;
using UserManagement.Services.Data;

// Define the Post class with a static method called CreateUser for creating a new user
internal static class Post
{
    // Handle creating a new user based on data from the request body
    public static async Task CreateUser(HttpRequest request, HttpResponse response, DatabaseService dbService)
    {
        try {
            using (SqlConnection connection = dbService.CreateConnection())
            {
                await connection.OpenAsync();

                // Deserialize the user data from the request body
                var user = await request.ReadFromJsonAsync<User>();

                // Generate a new unique ID for the user
                user.Id = Guid.NewGuid().ToString();

                using (SqlCommand command = new SqlCommand("spCreateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add the parameters for the stored procedure
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Age", user.Age);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    await command.ExecuteNonQueryAsync();

                    // Return a 201 Created response with the new user data
                    response.StatusCode = 201;
                    await response.WriteAsJsonAsync(user);
                }
            }
        } catch (Exception ex)
        {
            response.StatusCode = 500;
            await response.WriteAsync("An error occurred while creating the user.");
        }
    }
}


