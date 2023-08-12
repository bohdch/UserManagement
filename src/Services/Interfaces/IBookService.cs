using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetPopularBooks(int offset, int limit);
        Task<IEnumerable<BookViewModel>> GetBooksByCategory(int offset, int limit, string category);
        Task AddBooks(List<Book> books);
        Task AddRequestedPage(string category, int page);
        Task<bool> IsPageRequested(string category, int page);
    }
}
