using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookVerse.Services.Interfaces;
using BookVerse.Models;
using BookVerse.Models.ViewModels;
using BookVerse.Data;
using System.Security.Cryptography.X509Certificates;

namespace BookVerse.Services
{
    public class BookService : IBookService
    {
        private readonly BookVerseDbContext _dbContext;

        public BookService(BookVerseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookViewModel>> GetPopularBooks(int offset, int limit)
        {
            var popularBooks = await _dbContext.Books
                .AsNoTracking()
                .OrderByDescending(book => book.DownloadCount)
                .Skip(offset)
                .Take(limit)
                .Select(book => new BookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Authors = book.Authors,
                    Formats = book.Formats,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return popularBooks;
        }

        public async Task<IEnumerable<BookViewModel>> GetBooksByCategory(int offset, int limit, string category)
        {
            var query = _dbContext.Books
                .AsNoTracking()
                .OrderByDescending(book => book.DownloadCount)
                .Where(book =>
                    book.Bookshelves.Contains(category) ||
                    book.Subjects.Contains(category));

            var booksByCategory = await query
                .Skip(offset)
                .Take(limit)
                .Select(book => new BookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Authors = book.Authors,
                    Formats = book.Formats,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return booksByCategory;
        }

        public async Task<BookDetailsViewModel> GetBookDetails(int bookId)
        {
            Book book = await _dbContext.Books.FindAsync(bookId);

            if (book == null)
            {
                return null; 
            }

            var bookDetails = new BookDetailsViewModel
            {
                Id = book.Id,
                Description = book.Description,
                Subjects = book.Subjects,
                Bookshelves = book.Bookshelves,
                Languages = book.Languages,
            };

            return bookDetails;
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

        public async Task UpdateBookDetails(BookDetailsViewModel book)
        {
            Book result = await _dbContext.Books.FindAsync(book.Id);

            result.Description = book.Description;
            await _dbContext.SaveChangesAsync();
        }
    }
}
