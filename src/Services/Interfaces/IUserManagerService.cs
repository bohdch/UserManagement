using UserManagement.Services.Data;
using System.Data.SqlClient;
using System;

namespace UserManagement.Services.Data.Interfaces 
{
    internal interface IUserManagerService
    {
        Task GetAllUsers(HttpResponse response, DatabaseService dbService);
        Task CreateUser(HttpRequest request, HttpResponse response, DatabaseService dbService);
        Task UpdateUser(HttpRequest request, HttpResponse response, DatabaseService dbService);
        Task DeleteUser(string? id, HttpResponse response, DatabaseService dbService);
    }
}

