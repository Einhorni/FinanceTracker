﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyManagement.DbContexts;

#nullable disable

namespace MoneyManagement.Migrations
{
    [DbContext(typeof(FinanceContext))]
    [Migration("20240805155521_foreignkeys_eingetragen")]
    partial class foreignkeys_eingetragen
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MoneyManagement.Entities.AccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<int>("InterestInterval")
                        .HasColumnType("int");

                    b.Property<double>("InterestRate")
                        .HasColumnType("float");

                    b.Property<int>("InvestmentDuration")
                        .HasColumnType("int");

                    b.Property<string>("KindOfAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Overdraft")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MoneyManagement.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Name = "Art Supplies"
                        },
                        new
                        {
                            Name = "Games"
                        },
                        new
                        {
                            Name = "Books"
                        },
                        new
                        {
                            Name = "Clothing"
                        },
                        new
                        {
                            Name = "Hygiene"
                        },
                        new
                        {
                            Name = "Other Fees"
                        },
                        new
                        {
                            Name = "Groceries"
                        },
                        new
                        {
                            Name = "Household"
                        },
                        new
                        {
                            Name = "Insurance"
                        },
                        new
                        {
                            Name = "Other Hobbies"
                        },
                        new
                        {
                            Name = "Health"
                        },
                        new
                        {
                            Name = "Stream and Tv"
                        },
                        new
                        {
                            Name = "Phone"
                        },
                        new
                        {
                            Name = "House"
                        },
                        new
                        {
                            Name = "Taxes"
                        },
                        new
                        {
                            Name = "Car & Fuel"
                        },
                        new
                        {
                            Name = "Salary"
                        },
                        new
                        {
                            Name = "Present"
                        },
                        new
                        {
                            Name = "Cashback"
                        },
                        new
                        {
                            Name = "Transer"
                        });
                });

            modelBuilder.Entity("MoneyManagement.Entities.TransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FromAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ToAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MoneyManagement.Entities.TransactionEntity", b =>
                {
                    b.HasOne("MoneyManagement.Entities.AccountEntity", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("MoneyManagement.Entities.AccountEntity", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
