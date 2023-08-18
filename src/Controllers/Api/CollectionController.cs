using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookVerse.Services.Interfaces;
using BookVerse.Services.Interfaces;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Controllers.Api
{
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        // Get all existing book collections
        [HttpGet("/api/collections")]
        public async Task<IActionResult> GetCollections([FromQuery] string userId)
        {
            var collections = await _collectionService.GetCollections(userId);
            return Ok(collections);
        }

        [HttpGet("api/collection/books")]
        public async Task<IActionResult> GetBooksFromCollection([FromQuery] int collectionId)
        {
            var books = await _collectionService.GetBooksFromCollection(collectionId);
            return Ok(books);
        }

        // Add a new collection
        [HttpPost("/api/collection")]
        public async Task<IActionResult> AddCollection([FromQuery] string title, [FromQuery] string userId)
        {
            await _collectionService.AddCollection(title, userId);
            return Ok();
        }

        // Add book to collection
        [HttpPost("/api/collection/add-book")]
        public async Task<IActionResult> AddBookToCollection([FromQuery] int bookId, [FromQuery] int collectionId)
        {
            await _collectionService.AddBookToCollection(bookId, collectionId);
            return Ok();
        }
    }
}