using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Migrations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingSystemAPIs.Repository
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly DataContext _context = null!;

        // Constructor to initialize the database context via dependency injection
        public TransactionRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            // Add the new transaction to the context
            await _context.Transactions.AddAsync(transaction);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the added transaction
            return transaction;
        }

        public async Task DeleteAsync(Guid id)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fetch the transaction by its ID
                Transaction tempTransaction = await GetByIdAsync(id);

                // Remove the transaction from the context
                _context.Transactions.Remove(tempTransaction);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to ensure the deletion is applied
                await dBtransaction.CommitAsync();
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            // Return the list of all transactions in the Transactions table
            return await _context.Transactions
                .Include(t => t.Rate)
                .ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            // Try to find the transaction by its ID
            Transaction? transaction = await _context.Transactions
                .Include(t => t.Rate)
                .FirstOrDefaultAsync(t=> t.Id == id);

            // If the transaction is not found, throw a custom exception
            if (transaction == null)
            {
                throw new TransactionNotFoundException("Transaction is not found");
            }
            return transaction; // Return the found transaction
        }

        public async Task<Transaction> UpdateAsync(Guid id, Transaction transaction)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fetch the current transaction by its ID
                Transaction tempTransaction = await GetByIdAsync(id);

                // Preserve the ID of the original transaction to prevent overwriting it
                transaction.Id = tempTransaction.Id;

                // Detach the original transaction to avoid conflicts during update
                _context.Entry(tempTransaction).State = EntityState.Detached;

                // Mark the new transaction as modified
                _context.Entry(transaction).State = EntityState.Modified;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to ensure the update is applied
                await dBtransaction.CommitAsync();

                // Return the updated transaction
                return transaction;
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }
    }
}
