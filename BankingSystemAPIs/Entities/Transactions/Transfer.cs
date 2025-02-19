using System.ComponentModel.DataAnnotations.Schema;
using BankingSystemAPIs.Entities.Accounts;

namespace BankingSystemAPIs.Entities.Transactions
{
    // The Transfer class inherits from the Transaction class and represents a transfer transaction.
    // A transfer involves moving funds from one account to another, but this class does not define additional properties or methods,
    // as it uses the base class' properties.
    public class Transfer : Transaction
    {
        // Constructor to initialize the transfer transaction with an amount and a currency.
        // It calls the base class constructor to set the Amount and Currency, and automatically sets the Timestamp.
        public Transfer(decimal amount, Guid rateId)
            : base(amount, rateId)
        {
        }
    }
}
