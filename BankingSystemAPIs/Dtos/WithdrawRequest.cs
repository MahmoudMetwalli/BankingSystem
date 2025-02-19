using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{
    // The WithdrawRequest class is used as a Data Transfer Object (DTO) to transfer withdrawal request data.
    // It contains the necessary fields to process a withdrawal from a bank account.
    public class WithdrawRequest
    {
        // The unique identifier for the account from which the withdrawal is to be made
        // It is required and cannot be null or empty.
        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        // The amount to withdraw from the account
        // It must be a positive value (greater than zero).
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // The Currency is required
        [Required(ErrorMessage = "Rate ID is required.")]
        public Guid RateId { get; set; }
    }
}
