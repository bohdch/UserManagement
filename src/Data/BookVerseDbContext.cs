using System;
using Microsoft.EntityFrameworkCore;
using BookVerse.Models;

namespace BookVerse.Data
{
    public class BookVerseDbContext : DbContext
    {
        // Tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admin { get; set; } = null!;

        public BookVerseDbContext(DbContextOptions<BookVerseDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

