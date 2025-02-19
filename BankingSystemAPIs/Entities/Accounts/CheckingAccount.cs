using BankingSystemAPIs.Exceptions;

namespace BankingSystemAPIs.Entities.Accounts
{
    // The CheckingAccount class represents a checking account for a client, 
    // which includes an overdraft facility in addition to the standard account features.
    public class CheckingAccount : Account
    {
        // The overdraft limit for the checking account (default is 500)
        public decimal Overdraft { get; set; } = 500;

        // Lock object to ensure thread safety in Withdraw and Transfer methods
        private readonly object _lock = new object();

        // Constructor to initialize the checking account with balance, account number, 
        // client ID, currency, and optional overdraft limit
        public CheckingAccount(decimal balance, int accountNumber, Guid clientId, Guid rateId, decimal overdraft = 500)
            : base(balance, accountNumber, clientId, rateId)
        {
            Overdraft = overdraft; // Set the overdraft limit, default is 500
        }

        // Override Withdraw method to account for overdraft limit.
        // A withdrawal can succeed if the amount is less than or equal to the sum of the balance and overdraft limit.
        public override void Withdraw(decimal amount)
        {
            lock (_lock) // Ensures thread-safety during withdrawal
            {
                if (amount <= 0)
                    throw new NotPositiveException("Withdraw amount must be positive."); // Throw exception if the amount is non-positive
                if (amount <= Balance + Overdraft) // Check if the balance + overdraft limit is enough for the withdrawal
                {
                    Balance -= amount; // Deduct the amount from the balance
                    return;
                }
                throw new InsufficientFundsException("Insufficient Funds"); // Throw exception if there are not enough funds
            }
        }

        // Override Transfer method to ensure the source account (CheckingAccount) can withdraw the specified amount.
        // Currency conversion is handled before depositing into the target account.
        public override void Transfer(Account account, decimal amount, decimal senderRate, decimal receiverRate)
        {
            base.Withdraw(amount); // Withdraw the amount from the current account
            decimal transferAmount = amount / senderRate * receiverRate; // Convert currency if needed
            account.Deposit(transferAmount); // Deposit the converted amount into the target account
        }

        // Override ToString method to provide a custom string representation of the CheckingAccount,
        // including the overdraft limit.
        public override string ToString()
        {
            return base.ToString() + $" , overdraft : {Overdraft}"; // Add overdraft information to the string representation
        }
    }
}
