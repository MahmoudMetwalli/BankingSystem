using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Services
{
    public class TransactionService
    {
        // Database context for interacting with the database
        private readonly TransactionRepository _transactionRepository = null!;

        // Constructor that accepts the DataContext for dependency injection
        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // Retrieves a single transaction by its ID (returns the full transaction entity)
        public async Task<Transaction> Get(Guid id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        // Retrieves all transactions, including sender and receiver details
        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _transactionRepository.GetAllAsync();
        }

        // Adds a new transaction to the database
        public async Task<Transaction> Add(Transaction transaction)
        {
            return await _transactionRepository.AddAsync(transaction);
        }

        // Updates an existing transaction by its ID
        public async Task<Transaction> Update(Guid id, Transaction transaction)
        {
            return await _transactionRepository.UpdateAsync(id, transaction);
        }

        // Deletes a transaction by its ID
        public async Task Delete(Guid id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}
