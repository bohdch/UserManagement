using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserManagementDbContext : DbContext
    {
        // Table
        public DbSet<User> Users { get; set; }

        public UserManagementDbContext()
        {
            Database.EnsureCreated();   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database = UserManagment;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

