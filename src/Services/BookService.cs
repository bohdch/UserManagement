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
                .OrderByDescending(book => book.DownloadCount)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<Book>> GetBooksByCategory(int offset, int limit, string category)
        {
            List<Book> booksByCategory = await _dbContext.Books
                   .OrderByDescending(book => book.DownloadCount)
                   .Where(book =>
                       ((string)(object)book.Bookshelves).Contains(category) ||
                       ((string)(object)book.Subjects).Contains(category))
                   .Skip(offset)
                   .Take(limit)
                   .ToListAsync();

            return booksByCategory;
        }

        public async Task AddBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                var existingBook = await _dbContext.Books.FindAsync(book.Id);

                if (existingBook == null)
                {
                    _dbContext.Books.Add(book);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRequestedPage(string category, int page)
        {
            bool pageExists = await _dbContext.RequestedPages.AnyAsync(rp => rp.Category == category && rp.PageNumber == page);

            if (!pageExists)
            {
                var requestedPage = new RequestedPage { Category = category, PageNumber = page };
                _dbContext.RequestedPages.Add(requestedPage);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsPageRequested(string category, int page)
        {
            return await _dbContext.RequestedPages.AnyAsync(rp => rp.Category == category && rp.PageNumber == page);
        }
    }
}
