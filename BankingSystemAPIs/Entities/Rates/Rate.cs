using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Entities.Rates
{
    // The Rate class represents the exchange rate for a specific currency.
    // It includes the currency code (ISO 4217 standard) and the exchange rate associated with it.
    [Index(nameof(Currency), IsUnique = true)]
    public class Rate
    {
        // The Currency property represents the ISO 4217 currency code (e.g., "USD" for US Dollar).
        // It is required, and its maximum length is limited to 3 characters (standard for ISO currency codes).
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "USD"; // Default to "USD" if not specified

        // The CurrencyRate property represents the exchange rate of the currency against the base currency (e.g., USD).
        // The default rate is set to 1, meaning it is the base currency itself.
        public decimal CurrencyRate { get; set; } = 1; // Default rate is 1 (for the base currency)
        public Rate()
        {
        }
        public Rate(string currency, decimal currencyRate)
        {
            Currency = currency;
            CurrencyRate = currencyRate;
        }
    }
}

