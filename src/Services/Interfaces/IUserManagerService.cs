using UserManagement.Services;
using System.Data.SqlClient;
using System;
using UserManagement.Data;

namespace UserManagement.Services.Interfaces 
{
    internal interface IUserManagerService
    {
        Task GetAllUsers(HttpResponse response, UserManagementDbContext dbService);
        Task GetUser(string? id, HttpResponse response, UserManagementDbContext dbContext);
        Task CreateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbService);
        Task UpdateUser(HttpRequest request, HttpResponse response, UserManagementDbContext dbService);
        Task DeleteUser(string? id, HttpResponse response, UserManagementDbContext dbService);
    }
}

