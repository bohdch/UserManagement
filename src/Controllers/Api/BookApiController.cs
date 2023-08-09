using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookVerse.Services.Interfaces;
using BookVerse.Services;
using BookVerse.Models;

namespace BookVerse.Controllers.Api
{
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly IBookService _bookService;
        const int itemsPerPage = 8;

        public BookApiController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("/api/books/popular/{page}")]
        public async Task<IActionResult> GetPopularBooks(int page = 1)
        {
            var offset = (page - 1) * itemsPerPage;
            var books = await _bookService.GetPopularBooks(offset, itemsPerPage);

            return Ok(books);
        }

        [HttpGet("/api/books/{category}/{page}")]
        public async Task<IActionResult> GetBooksByCategory(string category, int page = 1)
        {
            var offset = (page - 1) * itemsPerPage;
            var books = await _bookService.GetBooksByCategory(offset, itemsPerPage, category);

            return Ok(books);
        }

        [HttpPost("/api/books/add")]
        public async Task<IActionResult> AddBooks(List<Book> books)
        {
            await _bookService.AddBooks(books);
            return Ok();
        }

        [HttpGet("/api/books/{category}/{page}/requested")]
        public async Task<IActionResult> CheckRequestedPage(string category, int page)
        {
            bool isRequested = await _bookService.IsPageRequested(category, page);

            if(!isRequested)
            {
                await _bookService.AddRequestedPage(category, page);
            }
            return Ok(new { requested = isRequested });
        }
    }
}
