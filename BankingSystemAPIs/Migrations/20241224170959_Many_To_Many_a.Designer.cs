﻿// <auto-generated />
using System;
using BankingSystemAPIs.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankingSystemAPIs.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241224170959_Many_To_Many_a")]
    partial class Many_To_Many_a
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BankingSystemAPIs.Entities.Accounts.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountNumber")
                        .HasColumnType("integer");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("AccountType").HasValue("Account");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.AcountsTransactions.AccountTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Source")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TransactionId");

                    b.ToTable("AccountsTransactions");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Rates.Rate", b =>
                {
                    b.Property<string>("Currency")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<decimal>("CurrencyRate")
                        .HasColumnType("numeric");

                    b.HasKey("Currency");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.HasKey("Id");

                    b.ToTable("Transactions");

                    b.HasDiscriminator<string>("TransactionType").HasValue("Transaction");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Accounts.CheckingAccount", b =>
                {
                    b.HasBaseType("BankingSystemAPIs.Entities.Accounts.Account");

                    b.Property<decimal>("Overdraft")
                        .HasColumnType("numeric");

                    b.HasDiscriminator().HasValue("CheckingAccount");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Accounts.SavingsAccount", b =>
                {
                    b.HasBaseType("BankingSystemAPIs.Entities.Accounts.Account");

                    b.Property<decimal>("Interest")
                        .HasColumnType("numeric");

                    b.HasDiscriminator().HasValue("SavingsAccount");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Transactions.Deposit", b =>
                {
                    b.HasBaseType("BankingSystemAPIs.Entities.Transactions.Transaction");

                    b.HasDiscriminator().HasValue("Deposit");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Transactions.Transfer", b =>
                {
                    b.HasBaseType("BankingSystemAPIs.Entities.Transactions.Transaction");

                    b.HasDiscriminator().HasValue("Transfer");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Transactions.Withdraw", b =>
                {
                    b.HasBaseType("BankingSystemAPIs.Entities.Transactions.Transaction");

                    b.HasDiscriminator().HasValue("Withdraw");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Accounts.Account", b =>
                {
                    b.HasOne("BankingSystemAPIs.Entities.User.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.AcountsTransactions.AccountTransaction", b =>
                {
                    b.HasOne("BankingSystemAPIs.Entities.Accounts.Account", "Account")
                        .WithMany("AccountsTransactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankingSystemAPIs.Entities.Transactions.Transaction", "Transaction")
                        .WithMany("AccountsTransactions")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Accounts.Account", b =>
                {
                    b.Navigation("AccountsTransactions");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.Transactions.Transaction", b =>
                {
                    b.Navigation("AccountsTransactions");
                });

            modelBuilder.Entity("BankingSystemAPIs.Entities.User.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
