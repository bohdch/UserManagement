using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookVerse.Services.Interfaces;
using BookVerse.Services;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Controllers
{
    public class PageTrackingController : Controller
    {
        private readonly IPageTrackingService _pageTrackingService;
        const int itemsPerPage = 8;

        public PageTrackingController(IPageTrackingService pageTrackingService)
        {
            _pageTrackingService = pageTrackingService;
        }

        [HttpGet("/api/books/{category}/{page}/requested")]
        public async Task<IActionResult> IsPageRequested(string category, int page)
        {
            bool isRequested = await _pageTrackingService.IsPageRequested(category, page);

            if (!isRequested)
            {
                await _pageTrackingService.AddRequestedPage(category, page);
            }
            return Ok(new { requested = isRequested });
        }
    }
}