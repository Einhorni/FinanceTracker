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
                new CategoryEntity { Name = "Art Supplies", Expense = true },
                new CategoryEntity { Name = "Games", Expense = true },
                new CategoryEntity { Name = "Books", Expense = true },
                new CategoryEntity { Name = "Clothing", Expense = true },
                new CategoryEntity { Name = "Hygiene", Expense = true },
                new CategoryEntity { Name = "Other Fees", Expense = true },
                new CategoryEntity { Name = "Groceries", Expense = true },
                new CategoryEntity { Name = "Household", Expense = true },
                new CategoryEntity { Name = "Insurance", Expense = true },
                new CategoryEntity { Name = "Other Hobbies", Expense = true },
                new CategoryEntity { Name = "Health", Expense = true },
                new CategoryEntity { Name = "Stream and Tv", Expense = true },
                new CategoryEntity { Name = "Phone", Expense = true },
                new CategoryEntity { Name = "House", Expense = true },
                new CategoryEntity { Name = "Taxes", Expense = true },
                new CategoryEntity { Name = "Car & Fuel", Expense = true },
                new CategoryEntity { Name = "Salary", Expense = true },
                new CategoryEntity { Name = "Present", Expense = true },
                new CategoryEntity { Name = "Cashback", Expense = false },
                new CategoryEntity { Name = "Transfer", Expense = false },
                new CategoryEntity { Name = "Income", Expense = false }
                );

            modelBuilder.Entity<AccountEntity>()
                .HasMany(a => a.Transactions)
                .WithOne(a => a.Account);

            base.OnModelCreating(modelBuilder);
        }
    }
}