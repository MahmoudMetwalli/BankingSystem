using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{
    // The TransferRequest class is used as a Data Transfer Object (DTO) to transfer transfer request data.
    // It contains the necessary fields to process a transfer between accounts.
    public class TransferRequest
    {
        // The unique identifier for the account from which the funds will be transferred.
        // It is required and cannot be null or empty.
        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        // The unique identifier for the target account to which the funds will be transferred.
        // It is required and cannot be null or empty.
        [Required(ErrorMessage = "Target Account ID is required.")]
        public Guid TargetAccountId { get; set; }

        // The amount to transfer between the accounts.
        // It must be a positive value (greater than zero).
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // The Currency is required
        [Required(ErrorMessage = "Rate ID is required.")]
        public Guid RateId { get; set; }
    }
}
