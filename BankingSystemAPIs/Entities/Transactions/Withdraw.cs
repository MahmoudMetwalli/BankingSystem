namespace BankingSystemAPIs.Entities.Transactions
{
    // The Withdraw class inherits from the Transaction class and represents a withdrawal transaction.
    // A withdrawal involves taking funds out of an account, and like the Transfer class, 
    // this class uses the properties defined in the base Transaction class.
    public class Withdraw : Transaction
    {
        // Constructor to initialize a withdrawal transaction with a specific amount and currency.
        // It calls the base class constructor to set the Amount and Currency, and automatically sets the Timestamp.
        public Withdraw(decimal amount, Guid rateId)
            : base(amount, rateId)
        {
        }
    }
}
