using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserManagementDbContext : DbContext
    {
        // Tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admin { get; set; } = null!;

        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

