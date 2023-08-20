using Microsoft.EntityFrameworkCore;
using BookVerse.Models;
using Newtonsoft.Json; 

namespace BookVerse.Data
{
    public class BookVerseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<RequestedPage> RequestedPages { get; set; } = null!;
        public DbSet<Collection> Collections { get; set; } = null!;

        public BookVerseDbContext(DbContextOptions<BookVerseDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nonclustered indexes
            modelBuilder.Entity<Book>()
                .HasIndex(book => book.Bookshelves);

            modelBuilder.Entity<Book>()
                .HasIndex(book => book.DownloadCount);


            modelBuilder.Entity<Book>()
                .Property(e => e.Formats)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
        }
    }
}
