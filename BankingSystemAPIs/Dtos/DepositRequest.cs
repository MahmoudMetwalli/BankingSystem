using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{
    // The DepositRequest class is used for representing a request to deposit money into an account.
    // It includes necessary information like the account ID, amount, and currency to process the deposit.
    public class DepositRequest
    {
        // The AccountId is required to identify the account into which the deposit will be made.
        // It is mandatory, so the user must provide an AccountId when making the request.
        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        // The Amount is required for the deposit operation and must be a positive value (greater than 0).
        // The Range attribute ensures that the amount is at least 0.01 and not less.
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // The Currency is required
        [Required(ErrorMessage = "Rate ID is required.")]
        public Guid RateId { get; set; } 
    }
}
