using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.Transactions;

namespace BankingSystemAPIs.Entities.AcountsTransactions
{
    // The AccountTransaction class represents a relationship between an account and a transaction.
    // It is used to track which account is associated as either a source or destination for a given transaction.
    public class AccountTransaction
    {
        // Unique identifier for the account associated with this transaction.
        public Guid AccountId { get; set; }

        // Unique identifier for the transaction in question.
        public Guid TransactionId { get; set; }

        // Navigation property to access the Account associated with this AccountTransaction.
        public virtual Account Account { get; set; } = null!;

        // Navigation property to access the Transaction associated with this AccountTransaction.
        public virtual Transaction Transaction { get; set; } = null!;

        // Boolean indicating whether this account is the source (true) or destination (false) for the transaction.
        public bool Source { get; set; }

        // Default constructor for the AccountTransaction class.
        public AccountTransaction() { }

        // Constructor to initialize an AccountTransaction with specific account ID, transaction ID, and source status.
        public AccountTransaction(Guid accountId, Guid transactionId, bool source)
        {
            AccountId = accountId;
            TransactionId = transactionId;
            Source = source;
        }
    }
}
