using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Services
{
    public class RateService
    {
        // Database context for interacting with the database
        private readonly RateRepository _rateRepository = null!;

        // Constructor that accepts the DataContext for dependency injection
        public RateService(RateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        // Retrieves a single rate by currency
        public async Task<Rate> Get(Guid id)
        {

            return await _rateRepository.GetByIdAsync(id); // Return the found rate
        }

        // Retrieves all rates from the Rates table
        public async Task<IEnumerable<Rate>> GetAll()
        {
            // Return the list of all rates in the Rates table
            return await _rateRepository.GetAllAsync(); 
        }

        // Adds a new rate to the Rates table
        public async Task<Rate> Add(Rate rate)
        {
            return await _rateRepository.AddAsync(rate); // Return the added rate
        }

        // Updates an existing rate by its currency
        public async Task<Rate> Update(Guid id, Rate rate)
        {
            return await _rateRepository.UpdateAsync(id, rate); // Return the updated rate
        }

        // Deletes a rate by its currency
        public async Task Delete(Guid id)
        {
            await _rateRepository.DeleteAsync(id); // Delete the rate
        }
        public async Task<Rate> GetExchangeRate(string currency)
        {
            return await _rateRepository.GetExchangeRate(currency);
        }
        public decimal Convert(decimal amount, decimal transactionRate, decimal accountRate)
        {
            // The conversion is calculated by dividing the amount by the transaction rate
            // and then multiplying by the account's currency rate to convert it into the account's currency.
            return amount / transactionRate * accountRate;
        }
    }
}
