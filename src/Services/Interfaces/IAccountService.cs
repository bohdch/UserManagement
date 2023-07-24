using Microsoft.AspNetCore.Mvc;

namespace BookVerse.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IActionResult> SignIn(string email, string password);
    }
}
