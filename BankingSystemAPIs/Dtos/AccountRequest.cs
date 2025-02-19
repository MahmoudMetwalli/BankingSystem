using System;
using System.ComponentModel.DataAnnotations;
using BankingSystemAPIs.Entities.Rates;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAPIs.Dtos
{
    // Abstract class that defines common properties for all account types (checking, savings, etc.).
    public abstract class AccountRequest
    {
        // The UserId is required to associate the account with a user (Client).
        // The error message is customized to ensure the UserId is provided.
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        // The RateId field represents the currency of the account
        [Required]
        public Guid RateId { get; set; }

        // The AccountNumber is required and must be generated automatically when creating the account.
        // It's marked with DatabaseGeneratedOption.Identity to indicate that the database will handle generating the account number.
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int AccountNumber { get; set; }

        // The Balance field is required, and it must be a positive value.
        // The error message is customized to ensure the balance is greater than zero.
        [Required(ErrorMessage = "Balance is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Balance { get; set; }
    }
}
