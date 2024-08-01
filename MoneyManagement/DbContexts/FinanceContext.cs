using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.DbContexts
{
    public class FinanceContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Finances.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasData(
                new Category { Name = "Art Supplies" },
                new Category { Name = "Games" },
                new Category { Name = "Books" },
                new Category { Name = "Clothing" },
                new Category { Name = "Hygiene" },
                new Category { Name = "Other Fees" },
                new Category { Name = "Groceries" },
                new Category { Name = "Household" },
                new Category { Name = "Insurance" },
                new Category { Name = "Other Hobbies" },
                new Category { Name = "Health" },
                new Category { Name = "Stream and Tv" },
                new Category { Name = "Phone" },
                new Category { Name = "House" },
                new Category { Name = "Taxes" },
                new Category { Name = "Car & Fuel" },
                new Category { Name = "Salary" },
                new Category { Name = "Present" },
                new Category { Name = "Cashback" },
                new Category { Name = "Transer" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}