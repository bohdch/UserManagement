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

        public async Task<IEnumerable<Collection>> GetCollections(string userId)
        {
            List<Collection> collections = await _dbContext.Collections
                    .Where(c => c.User.Id == userId)
                    .ToListAsync();

            return collections;
        }

        public async Task<IEnumerable<BookViewModel>> GetBooksFromCollection(int collectionId)
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

        public async Task<int> AddCollection(string title, string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var collection = new Collection
            {
                Title = title,
                User = user,
            };

            _dbContext.Add(collection);
            await _dbContext.SaveChangesAsync();

            return collection.Id; // Return the collection's ID after it's been added
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

        public async Task RenameCollection(int collectionId, string newTitle)
        {
            var collection = await _dbContext.Collections.FindAsync(collectionId);

            collection.Title = newTitle;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCollection(int collectionId)
        {
            var collection = await _dbContext.Collections.FindAsync(collectionId);
            
            _dbContext.Collections.Remove(collection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookFromCollection(int bookId, int collectionId)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            var collection = await _dbContext.Collections.FindAsync(collectionId);

            // Load the 'Books' collection of the provided 'collection' entity
            await _dbContext.Entry(collection).Collection(c => c.Books).LoadAsync();

            // Load the 'Collections' collection of the provided 'book' entity
            await _dbContext.Entry(book).Collection(b => b.Collections).LoadAsync();

            collection.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }
}