using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{
    // The TransactionsWithType class is used to represent a transaction along with its associated type and other details.
    // It is used as a Data Transfer Object (DTO) to transfer information about transactions with their type (e.g., deposit, withdrawal, transfer).
    public class TransactionDetails
    {
        // The unique identifier for the sender of the transaction (i.e., the account initiating the transaction).
        // This is a required field and should be provided.
        public Guid AccountId { get; set; }

        // The unique identifier for the receiver of the transaction (i.e., the account receiving the funds).
        // It is optional, as some transaction types (e.g., withdrawals) may not have a receiver.
        public Guid? ReceiverId { get; set; }

        // The unique identifier for the transaction. This is a required field and should be provided.
        public Guid TransactionId { get; set; }

        // The type of the transaction, such as "Deposit", "Withdraw", or "Transfer".
        // It is a required field, and defaults to an empty string if not provided.
        public string TransactionType { get; set; } = string.Empty;

        // The amount of money involved in the transaction. It is a required field and should be provided.
        public decimal Amount { get; set; }

        // The currency of the transaction, represented by the ISO currency code (e.g., "USD", "EUR").
        // It is a required field, and defaults to an empty string if not provided.
        public string Currency { get; set; } = string.Empty;

        // The date and time when the transaction was performed.
        // This is a required field and should be provided.
        public DateTime TransactionDate { get; set; }
    }
}
