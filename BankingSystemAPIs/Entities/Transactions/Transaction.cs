using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Rates;

namespace BankingSystemAPIs.Entities.Transactions
{
    // The Transaction class serves as the base class for different types of transactions,
    // such as Deposit and Withdrawal. It contains common properties and behavior for transactions.
    public abstract class Transaction
    {
        // Unique identifier for each transaction. This will be generated automatically.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // A collection of associated account transactions for the current transaction.
        // It is marked as virtual to allow lazy loading of related data.
        public virtual List<AccountTransaction> AccountsTransactions { get; set; } = null!;

        // The amount involved in the transaction.
        public decimal Amount { get; set; }

        // The currency for the transaction.
        public Guid RateId { get; set; }

        // A reference to the rate associated with the currency. This is linked via the "Currency" foreign key.
        [ForeignKey("RateId")]
        public Rate Rate { get; set; } = null!;

        // Timestamp when the transaction occurred. Defaults to the current UTC time.
        public DateTime Timestamp { get; set; }

        // Default constructor, which is required for Entity Framework.
        public Transaction() { }

        // Parameterized constructor to initialize a transaction with a specified amount and currency.
        // Sets the timestamp to the current UTC time.
        public Transaction(decimal amount, Guid rateId)
        {
            Amount = amount;
            Timestamp = DateTime.UtcNow; // Sets the timestamp to the current UTC time.
            RateId = rateId;
        }
    }
}
