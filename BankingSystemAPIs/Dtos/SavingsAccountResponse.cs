using System;
using System.ComponentModel.DataAnnotations;
using BankingSystemAPIs.Entities.Rates;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAPIs.Dtos
{
    // The SavingsAccountRequest class extends AccountRequest and is used to represent the request data needed to create or update a savings account.
    // It includes an additional property, Interest, which specifies the interest rate for the savings account.
    public class SavingsAccountResponse : AccountResponse
    {
        // The interest rate of the savings account. It is required and must be within the range of 0.01 to 100.
        // The validation ensures that the interest rate is a positive number and not greater than 100.
        [Required(ErrorMessage = "Interest is required")]  // The Interest field is mandatory.
        [Range(0.01, 100, ErrorMessage = "Interest must be greater than zero and less than 100")] // Ensures that the interest rate is between 0.01 and 100.
        public decimal Interest { get; set; }
    }
}
