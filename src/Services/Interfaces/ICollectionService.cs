using Microsoft.AspNetCore.Mvc;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<Collection>> GetCollections();

        Task<IEnumerable<BookViewModel>> GetBooksFromCollection([FromQuery] int collectionId);

        Task AddCollection(string title, Guid userId);

        Task AddBookToCollection(int bookId, int collectionId);
    }
}
