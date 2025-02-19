using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Data
{
    // DataContext serves as the DbContext for the banking system's database
    public class DataContext : DbContext
    {
        // DbSet properties represent the database tables
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<AccountTransaction> AccountsTransactions { get; set; }

        // Constructor accepting DbContextOptions and passing them to the base class
        // Allows configuring the context, such as the database provider and connection string
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // OnModelCreating method is used to configure the model and relationships between entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the inheritance for 'Transaction' using a discriminator column
            // Each type of transaction (Transfer, Deposit, Withdraw) will be identified by 'TransactionType' column
            modelBuilder.Entity<Transaction>()
                .HasDiscriminator<string>("TransactionType")
                .HasValue<Transfer>("Transfer")
                .HasValue<Deposit>("Deposit")
                .HasValue<Withdraw>("Withdraw");

            // Configuring the inheritance for 'Account' using a discriminator column
            // Each type of account (SavingsAccount, CheckingAccount) will be identified by 'AccountType' column
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<SavingsAccount>("SavingsAccount")
                .HasValue<CheckingAccount>("CheckingAccount");

            // Configuring RowVersion property for 'Account' to use it as a concurrency token
            // This ensures optimistic concurrency control to avoid data conflicts during updates
            modelBuilder.Entity<Account>()
                .Property(a => a.RowVersion)
                .IsConcurrencyToken(); // Marks RowVersion as the concurrency token for 'Account'

            // Configuring a composite primary key for the 'AccountTransaction' entity
            // The primary key consists of both AccountId and TransactionId
            modelBuilder.Entity<AccountTransaction>()
                .HasKey(at => new { at.AccountId, at.TransactionId });

            // Configuring relationships between 'AccountTransaction', 'Account', and 'Transaction'
            // Each 'AccountTransaction' is associated with one 'Account' and one 'Transaction'
            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.Account) // 'AccountTransaction' has one 'Account'
                .WithMany(a => a.AccountsTransactions) // One 'Account' can have many 'AccountTransactions'
                .HasForeignKey(at => at.AccountId); // Foreign key on 'AccountId'

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.Transaction) // 'AccountTransaction' has one 'Transaction'
                .WithMany(a => a.AccountsTransactions) // One 'Transaction' can have many 'AccountTransactions'
                .HasForeignKey(at => at.TransactionId); // Foreign key on 'TransactionId'
        }
    }
}
