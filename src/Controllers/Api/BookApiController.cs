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

        public BookApiController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("/api/popular-books/{page}")]
        public async Task<IActionResult> GetPopularBooks(int page = 1)
        {
            const int itemsPerPage = 8;

            var offset = (page - 1) * itemsPerPage;
            var newBooks = await _bookService.GetPopularBooks(offset, itemsPerPage);

            return Ok(newBooks);
        }

        [HttpPost("/api/popular-books/add")]
        public async Task<IActionResult> AddPopularBooks(List<Book> books)
        {
            await _bookService.AddPopularBooks(books);
            return Ok();
        }
    }
}
