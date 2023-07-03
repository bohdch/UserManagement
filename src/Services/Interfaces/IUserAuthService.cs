using UserManagement.Services;
using System.Data.SqlClient;
using System;
using UserManagement.Data;
using UserManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<IActionResult> SignIn(string email, string password);
        IActionResult SignUp();
    }
}
