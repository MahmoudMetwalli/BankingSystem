using BankingSystemAPIs.Entities.Rates;

namespace BankingSystemAPIs.Entities.Transactions
{
    // The Deposit class represents a deposit transaction, which is a type of transaction.
    // It inherits from the base class Transaction.
    public class Deposit : Transaction
    {
        // Constructor to initialize a Deposit transaction with the specified amount and currency.
        // The constructor calls the base class constructor to initialize the common properties of a transaction.
        public Deposit(decimal amount, Guid rateId)
            : base(amount, rateId) // Passes amount and currency to the base Transaction class constructor
        {
        }
    }
}
