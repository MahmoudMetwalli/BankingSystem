namespace BankingSystemAPIs.Entities.Accounts
{
    // The SavingsAccount class represents a savings account, which includes an interest rate 
    // to calculate the accumulated interest over time.
    public class SavingsAccount : Account
    {
        // The interest rate for the savings account (percentage)
        public decimal Interest { get; set; }

        // Lock object to ensure thread safety when updating the balance
        private readonly object _lock = new object();

        // Constructor to initialize the savings account with balance, account number, 
        // client ID, currency, and interest rate
        public SavingsAccount(decimal balance, int accountNumber, Guid clientId, Guid rateId, decimal interest)
            : base(balance, accountNumber, clientId, rateId)
        {
            Interest = interest; // Set the interest rate for the account
        }

        // Method to add interest to the account balance over a given number of years.
        // The balance is updated each year by adding the calculated interest based on the current balance.
        public void AddInterest(int years)
        {
            lock (_lock) // Ensures thread-safety during interest addition
            {
                // Loop through each year and add interest
                for (int i = 0; i < years; i++)
                {
                    Balance += (Interest / 100) * Balance; // Add the interest to the balance
                }
            }
        }

        // Method to calculate the interest accumulated over a given number of years without actually modifying the balance.
        // It returns the total interest accumulated over the specified years.
        public decimal InterestCalculation(int years)
        {
            decimal interestBalance = Balance; // Start with the current balance
            // Loop through each year and calculate interest
            for (int i = 0; i < years; i++)
            {
                interestBalance += (Interest / 100) * interestBalance; // Calculate interest for the current year
            }
            // Return the difference between the calculated balance and the original balance, i.e., the interest earned
            return interestBalance - Balance;
        }
    }
}
