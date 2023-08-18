using Microsoft.AspNetCore.Mvc;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<Collection>> GetCollections(string userId);

        Task<IEnumerable<BookViewModel>> GetBooksFromCollection(int collectionId);

        Task AddCollection(string title, string userId);

        Task AddBookToCollection(int bookId, int collectionId);
    }
}
