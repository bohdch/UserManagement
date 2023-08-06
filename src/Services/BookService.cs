using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookVerse.Services.Interfaces;
using BookVerse.Models;
using BookVerse.Data;

namespace BookVerse.Services
{
    public class BookService : IBookService
    {
        private readonly BookVerseDbContext _dbContext;

        public BookService(BookVerseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetPopularBooks(int offset, int limit)
        {
            List<Book> books = await _dbContext.Books
                .OrderByDescending(book => book.Download_count)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return books;
        }

        public async Task AddPopularBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                var existingBook = await _dbContext.Books.FindAsync(book.id);

                if (existingBook == null)
                {
                    _dbContext.Books.Add(book);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
