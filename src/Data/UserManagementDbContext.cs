using System;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Data
{
    public class UserManagementDbContext : DbContext
    {
        // Table

        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();   
        }

    }
}

