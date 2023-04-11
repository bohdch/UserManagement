using System.Text.Json;

// Define the Post class with a static method called CreateUser for creating a new user
internal static class Post {
    
    // Handle creating a new user based on data from the request body
    public static async Task CreateUser(HttpRequest request, HttpResponse response, UserService userService) {
        try
        {
            // Retrieve all existing users from the user service
            var users = userService.RetrieveAllUsers();

            // Deserialize the user data from the request body
            var user = await request.ReadFromJsonAsync<Person>();

            // If user data is not provided, return a 400 Bad Request response
            if (user is null)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Incorrect data" });
                return;
            }

            // Generate a new unique ID for the user
            user.Id = Guid.NewGuid().ToString();

            // Add the new user to the existing list of users
            users.Add(user);

            // Return a 201 Created response with the new user data
            response.StatusCode = 201; 
            await response.WriteAsJsonAsync(user);
        }
        catch (JsonException)
        {
            // Return a 400 Bad Request response if there is an error deserializing the user data
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Incorrect data" });
        }
        catch (Exception)
        {
            // Return a 500 Internal Server Error response if there is an unexpected error
            response.StatusCode = 500;
            await response.WriteAsJsonAsync(new { message = "Internal server error" });
        }
    }
}
