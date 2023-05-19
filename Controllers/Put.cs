// Define a static method called UpdateUser that handles updating an existing user
internal static class Put {
    // Handles updating an existing user by Id
    public static async Task UpdateUser(HttpRequest request, HttpResponse response, UserService userService){
    
        try {
            // Read user data from the request body
            var userData = await request.ReadFromJsonAsync<User>();
            if (userData != null) {
                // Retrieve all users from the data source
                var users = userService.RetrieveAllUsers();
                

                // Find user from all users by Id
                var user = users.FirstOrDefault(u => u.Id == userData.Id);

                if (user != null){
                    // Update user data
                    user.FirstName = userData.FirstName;
                    user.LastName = userData.LastName;
                    user.Age = userData.Age;
                    user.Email = userData.Email;

                    // Return updated user data
                    await response.WriteAsJsonAsync(user);
                } else {
                    // User not found, return 404
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new {message = "No user found"});
                }
            }
            else {
                // Incorrect data received, throw exception
                throw new Exception("Incorrect data");
            }
        }

        catch{ 
            // Error occurred while processing request, return 404
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new {message = "Incorrect data"});
        }
    }
}