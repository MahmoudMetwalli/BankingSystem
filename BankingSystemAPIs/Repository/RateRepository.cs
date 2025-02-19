using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Migrations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingSystemAPIs.Repository
{
    public class RateRepository : IRepository<Rate>
    {
        private readonly DataContext _context = null!;

        // Constructor to initialize the database context via dependency injection
        public RateRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CheckCurrency(string currency)
        {
            // Query the Rates table to find the rate for the given currency
            Rate? rate = await _context.Rates.FirstOrDefaultAsync(r => r.Currency == currency);
            // If the rate is not found, throw a custom exception
            if (rate != null)
            {
                throw new DuplicatedIdException("Duplicated Currency");
            }
        }
        public async Task<Rate> GetExchangeRate(string currency)
        {
            // Query the Rates table to find the rate for the given currency
            Rate? rate = await _context.Rates.FirstOrDefaultAsync(r => r.Currency == currency);
            // If the rate is not found, throw a custom exception
            if (rate == null)
            {
                throw new RateNotFoundException("Rate is not found");
            }
            return rate; // Return the found rate
        }
        public async Task<Rate> AddAsync(Rate rate)
        {
             await CheckCurrency(rate.Currency);
            await _context.Rates.AddAsync(rate);
             await _context.SaveChangesAsync(); // Save changes to the database
             return rate; // Return the added rate
        }

        public async Task DeleteAsync(Guid id)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the rate by its currency
                Rate tempRate = await GetByIdAsync(id);

                // Remove the rate from the Rates table
                _context.Rates.Remove(tempRate);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to ensure the deletion is applied
                await dBtransaction.CommitAsync();
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain data consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Rate>> GetAllAsync()
        {
            // Return the list of all rates in the Rates table
            return await _context.Rates.ToListAsync();
        }

        public async Task<Rate> GetByIdAsync(Guid id)
        {
            // Query the Rates table to find the rate for the given currency
            Rate? rate = await _context.Rates.FirstOrDefaultAsync(r => r.Id == id);

            // If the rate is not found, throw a custom exception
            if (rate == null)
            {
                throw new RateNotFoundException("Rate is not found");
            }
            return rate; // Return the found rate
        }

        public async Task<Rate> UpdateAsync(Guid id, Rate rate)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await CheckCurrency(rate.Currency);
                // Retrieve the existing rate by its currency
                Rate tempRate = await GetByIdAsync(id);

                // Preserve the currency of the original rate to prevent overwriting it
                rate.Id = tempRate.Id;

                // Detach the original rate entity to avoid conflicts during update
                _context.Entry(tempRate).State = EntityState.Detached;

                // Mark the new rate as modified
                _context.Entry(rate).State = EntityState.Modified;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to ensure the update is applied
                await dBtransaction.CommitAsync();

                // Return the updated rate
                return rate;
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain data consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }
    }
}
