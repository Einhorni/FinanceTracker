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
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FinanceTracker;Trusted_Connection=True;MultipleActiveResultSets=true");
            //"Server=(localdb)\\mssqllocaldb;Database=FinanceTracker;Trusted_Connection=True;MultipleActiveResultSets=true"
            //"Data Source=Finances.db"
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryEntity>()
                .HasData(
                new CategoryEntity { Name = "Art Supplies" },
                new CategoryEntity { Name = "Games" },
                new CategoryEntity { Name = "Books" },
                new CategoryEntity { Name = "Clothing" },
                new CategoryEntity { Name = "Hygiene" },
                new CategoryEntity { Name = "Other Fees" },
                new CategoryEntity { Name = "Groceries" },
                new CategoryEntity { Name = "Household" },
                new CategoryEntity { Name = "Insurance" },
                new CategoryEntity { Name = "Other Hobbies" },
                new CategoryEntity { Name = "Health" },
                new CategoryEntity { Name = "Stream and Tv" },
                new CategoryEntity { Name = "Phone" },
                new CategoryEntity { Name = "House" },
                new CategoryEntity { Name = "Taxes" },
                new CategoryEntity { Name = "Car & Fuel" },
                new CategoryEntity { Name = "Salary" },
                new CategoryEntity { Name = "Present" },
                new CategoryEntity { Name = "Cashback" },
                new CategoryEntity { Name = "Transer" }
                );

            modelBuilder.Entity<AccountEntity>()
                .HasMany(a => a.Transactions)
                .WithOne(a => a.Account);

            base.OnModelCreating(modelBuilder);
        }
    }
}