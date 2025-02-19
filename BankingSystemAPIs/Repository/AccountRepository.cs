using System.Security.Principal;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Migrations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingSystemAPIs.Repository
{
    public class AccountRepository
    {
        private readonly DataContext _context = null!;

        // Constructor to initialize the database context via dependency injection
        public AccountRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CheckAccountNumber(double accountNumber)
        {
            Account? existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (existingAccount != null)
            {
                // If account exists, throw an exception
                throw new AccountNumberException("Account Number is already exist");
            }
        }
        public async Task<Account> AddAsync(Account account)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if the account number is already taken
                await CheckAccountNumber(account.AccountNumber);
                // Add the new account to the database
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                // Commit the transaction
                await transaction.CommitAsync();
                return account;
            }
            catch
            {
                // Rollback if an error occurs
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get the account to be deleted
                Account tempAccount = await GetByIdAsync(id);
                // Remove the account from the database
                _context.Accounts.Remove(tempAccount);
                await _context.SaveChangesAsync();
                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback if an error occurs
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            // Return the list of all accounts in the Accounts table
            return await _context.Accounts
        .Include(a => a.Rate) // Include the Rate entity
        .Include(a => a.Client) // Include the Client entity
        .ToListAsync();
        }
        public async Task<IEnumerable<SavingsAccount>> GetAllSavingsAccountsAsync()
        {
            // Return the list of all accounts in the Accounts table
            return await _context.Accounts.OfType<SavingsAccount>().Include(a => a.Rate) // Include the Rate entity
        .Include(a => a.Client) // Include the Client entity
        .ToListAsync();
        }
        public async Task<IEnumerable<CheckingAccount>> GetAllCheckingAccountsAsync()
        {
            // Return the list of all accounts in the Accounts table
            return await _context.Accounts.OfType<CheckingAccount>().Include(a => a.Rate).Include(a => a.Client).ToListAsync();
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            Account? account = await _context.Accounts.Include(a => a.Rate) // Include the Rate entity
        .Include(a => a.Client).FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                throw new AccountNotFoundException("Account is not found");
            }
            return account;
        }

        public async Task<Account> UpdateAsync(Guid id, Account account)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get the account to be updated
                Account tempAccount = await GetByIdAsync(id);
                // Ensure the account number is unique
                if (tempAccount.AccountNumber != account.AccountNumber)
                {
                    await CheckAccountNumber(account.AccountNumber);
                }
                // Update the account
                account.Id = tempAccount.Id;
                _context.Entry(tempAccount).State = EntityState.Detached;
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                // Commit the transaction
                await transaction.CommitAsync();
                return account;
            }
            catch
            {
                // Rollback if an error occurs
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await operation(); // Execute the provided operation
                    await _context.SaveChangesAsync(); // Save changes
                    await transaction.CommitAsync(); // Commit transaction
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(); // Rollback on error
                    throw;
                }
            }
        }
    }
}
