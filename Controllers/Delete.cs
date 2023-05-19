// This class contains a static method for deleting a user
internal static class Delete {
    // method deletes an existing user by Id
    public static async Task DeleteUser(string? id, HttpResponse response, UserService userService){
    
    // Retrieve all users from the data source
    var users = userService.RetrieveAllUsers();
    User? user = null;

    // Find user from all users by Id
    foreach (User u in users){
        if (u.Id == id)
        {
            user = u;
            break;
        }
    }

    if (user != null){
        // Remove user from the list of users
        users.Remove(user);
        // Return the deleted user data
        await response.WriteAsJsonAsync(user);
    } else {
        // User not found, return 404
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "No user found" });
    }
    }
}