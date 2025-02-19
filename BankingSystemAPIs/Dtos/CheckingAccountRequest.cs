using System;
using System.ComponentModel.DataAnnotations;
using BankingSystemAPIs.Entities.Rates;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAPIs.Dtos
{
    // The CheckingAccountRequest class inherits from AccountRequest and represents the data needed
    // to create or update a checking account. This includes the overdraft limit specific to checking accounts.
    public class CheckingAccountRequest : AccountRequest
    {
        // The OverDraft property is required when creating or updating a checking account.
        // It must be a positive value greater than zero. The error message is customized to ensure the value is valid.
        [Required(ErrorMessage = "OverDraft is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "OverDraft must be greater than zero")]
        public decimal OverDraft { get; set; }
    }
}
