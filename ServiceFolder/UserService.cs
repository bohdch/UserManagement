internal class UserService
{
    // A data structure that represents all users
    private List<User> _users = new List<User>() {
        new User() {Id = Guid.NewGuid().ToString(), FirstName="Harold", LastName="Lowe", Age=30, Email = "hlowe@gmail.com"},
        new User() {Id = Guid.NewGuid().ToString(), FirstName="Nina", LastName = "Gibson", Age=21, Email = "ngibson@gmail.com"},
        new User() {Id = Guid.NewGuid().ToString(), FirstName="Michael", LastName = "Brewer", Age=28, Email = "mbrewer@gmail.com"}
    };

    public List<User> RetrieveAllUsers()
    {
        return _users;
    }
}
