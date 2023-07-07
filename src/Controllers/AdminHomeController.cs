using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
	public class AdminHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}