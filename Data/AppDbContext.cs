﻿using Microsoft.EntityFrameworkCore;
using PawsCare.Models;

namespace PawsCare.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
        public DbSet<User> Users { get; set; } = null!;
    }

}