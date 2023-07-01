using UserManagement.Services;
using System.Data.SqlClient;
using System;
using UserManagement.Data;

namespace UserManagement.Services.Interfaces 
{
    internal interface IUserManagerService
    {
        Task GetAllUsers(HttpResponse response);
        Task GetUser(string? id, HttpResponse response);
        Task CreateUser(HttpRequest request, HttpResponse response);
        Task UpdateUser(HttpRequest request, HttpResponse response);
        Task DeleteUser(string? id, HttpResponse response);
    }
}

