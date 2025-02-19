using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{

    public class RateRequest
    {

        [Required(ErrorMessage = "Currency is required.")]
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        [Required(ErrorMessage = "CurrencyRate is required.")]
        public decimal CurrencyRate { get; set; } = 1;
    }
}
