using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookVerse.Services.Interfaces;
using BookVerse.Services;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Controllers.Api
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        const int itemsPerPage = 8;

        public BookController(IBookService bookService)
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

        [HttpGet("/api/book-details/{bookId}")]
        public async Task<IActionResult> GetBookDetails(int bookId)
        {
            var details = await _bookService.GetBookDetails(bookId);
            return Ok(details);
        }

        [HttpPost("/api/books/add")]
        public async Task<IActionResult> AddBooks(List<Book> books)
        {
            await _bookService.AddBooks(books);
            return Ok();
        }

        [HttpPost("/api/book/add")]
        public async Task<IActionResult> AddBook(Book book)
        {
            await _bookService.AddBook(book);
            return Ok();
        }

        [HttpPut("/api/book-details/update")]
        public async Task<IActionResult> UpdateBookDetails(BookDetailsViewModel book)
        {
            await _bookService.UpdateBookDetails(book);
            return Ok();
        }
    }
}
