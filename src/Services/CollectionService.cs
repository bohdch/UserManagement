using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BookVerse.Services.Interfaces;
using BookVerse.Models;
using BookVerse.Models.ViewModels;
using BookVerse.Data;

namespace BookVerse.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly BookVerseDbContext _dbContext;

        public CollectionService(BookVerseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Collection>> GetCollections()
        {
            List<Collection> collections = await _dbContext.Collections.ToListAsync();  
            return collections;
        }

        public async Task<IEnumerable<BookViewModel>> GetBooksFromCollection([FromQuery] int collectionId)
        {
            var collection = await _dbContext.Collections
                .Include(c => c.Books)
                .ThenInclude(b => b.Authors) 
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            var books = collection.Books
                .Select(book => new BookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Authors = book.Authors, 
                    Formats = book.Formats,
                });

            return books;
        }

        public async Task AddCollection(string title, Guid userId)
        {
            var collection = new Collection
            {
                Title = title,
                UserId = userId,
            };

            _dbContext.Add(collection);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddBookToCollection(int bookId, int collectionId)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            var collection = await _dbContext.Collections.FindAsync(collectionId);

            if (collection.Books == null)
            {
                collection.Books = new List<Book>();
            }

            collection.Books.Add(book);

            await _dbContext.SaveChangesAsync();
        }
    }
}
