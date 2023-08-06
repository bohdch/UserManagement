using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookVerse.Models;

namespace BookVerse.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetPopularBooks(int offset, int limit);
        Task AddPopularBooks(List<Book> books);
    }
}
