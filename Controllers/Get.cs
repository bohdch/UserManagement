// Define the Get class with static methods to retrieve user data
internal static class Get 
{
    // Define a static method called GetAllUsers that retrieves all users from the user service and returns them in JSON format
    public static async Task GetAllUsers(HttpResponse response, UserService userService) 
    {
        var users = userService.RetrieveAllUsers();
        await response.WriteAsJsonAsync(users);
    }

    // Define a static method called GetUser that retrieves a single user from the user service based on their ID and returns them in JSON format
    public static async Task GetUser(string? id, HttpResponse response, UserService userService) 
    {
        var users = userService.RetrieveAllUsers();
        Person? user = null;
   
        // Loop through each user and find the one that matches the given ID
        foreach (Person person in users){
            if (person.Id == id)
            {
                user = person;
                break;
            }
        }
        
        // If a matching user was found, return a JSON response containing their data
        if (user != null){
            await response.WriteAsJsonAsync(user);
        } 
        // Otherwise, return a 404 Not Found response
        else {
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "No user found" });
        }
    }
}