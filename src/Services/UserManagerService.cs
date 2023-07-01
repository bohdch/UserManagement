using System;
using UserManagement.Services.Data.Interfaces;

namespace UserManagement.Services.Data
{
    public class UserManagerService : IUserManagerService
    {
        public Task GetAllUsers(HttpResponse response, DatabaseService dbService)
        {
            throw new NotImplementedException();
        }

        public Task CreateUser(HttpRequest request, HttpResponse response, DatabaseService dbService)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(HttpRequest request, HttpResponse response, DatabaseService dbService)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string? id, HttpResponse response, DatabaseService dbService)
        {
            throw new NotImplementedException();
        }
    }
}
