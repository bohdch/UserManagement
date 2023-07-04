using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<IActionResult> SignIn(string email, string password);
    }
}
