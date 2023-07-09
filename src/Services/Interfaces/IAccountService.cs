using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IActionResult> SignIn(string email, string password);
    }
}
