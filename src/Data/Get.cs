using System.Data.SqlClient;
using UserManagement.Services.Data;

// Define the Get class with static methods to retrieve user data
internal static class Get
{
    // Define a static method called GetAllUsers that retrieves all users from the database and returns them in JSON format
    public static async Task GetAllUsers(HttpResponse response, DatabaseService dbService)
    {
        try
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = dbService.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("spGetAllUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User user = new User()
                            {
                                Id = reader["Id"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Email = reader["Email"].ToString()
                            };
                            users.Add(user);
                        }
                        await response.WriteAsJsonAsync(users);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception here, you can log the error or return an appropriate response
            response.StatusCode = 500;
            await response.WriteAsync("An error occurred while retrieving users.");
        }
    }

    // Define a static method called GetUser that retrieves a single user from the database based on their ID and returns them in JSON format
    public static async Task GetUser(string? id, HttpResponse response, DatabaseService dbService)
    {
        try
        {
            using (SqlConnection connection = dbService.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("spGetOneUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            User user = new User()
                            {
                                Id = reader["Id"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Email = reader["Email"].ToString()
                            };
                            await response.WriteAsJsonAsync(user);
                        }
                        else
                        {
                            response.StatusCode = 404;
                            await response.WriteAsJsonAsync(new { message = "No user found" });
                        }
                    }
                }
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
