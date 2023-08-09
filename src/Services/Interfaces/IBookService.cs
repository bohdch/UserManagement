using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookVerse.Models;

namespace BookVerse.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetPopularBooks(int offset, int limit);
        Task<IEnumerable<Book>> GetBooksByCategory(int offset, int limit, string category);
        Task AddBooks(List<Book> books);
        Task AddRequestedPage(string category, int page);
        Task<bool> IsPageRequested(string category, int page);
    }
}
