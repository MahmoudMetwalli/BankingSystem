using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Repository
{
    public class AccountTransactionRepository
    {
        private readonly DataContext _context = null!;

        // Constructor to initialize the database context via dependency injection
        public AccountTransactionRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<AccountTransaction> AddAsync(AccountTransaction accountTransaction)
        {
            await _context.AccountsTransactions.AddAsync(accountTransaction);
            await _context.SaveChangesAsync();
            return accountTransaction;
        }
        public async Task<List<TransactionDetails>> GetAllAccountTransactions(Guid id)
        {
            var results = await _context.AccountsTransactions
                .Include(at => at.Transaction)
                .Where(at => at.AccountId == id)
                .Select(at => new TransactionDetails
                {
                    AccountId = at.Source ? at.AccountId : at.Transaction.AccountsTransactions.Where(at => at.Source).Select(at => at.AccountId).FirstOrDefault(),
                    ReceiverId = at.Source ? at.Transaction.AccountsTransactions.Where(at => !at.Source).Select(at => at.AccountId).FirstOrDefault() : at.AccountId,
                    Amount = at.Transaction.Amount,
                    Currency = at.Transaction.Rate.Currency,
                    TransactionDate = at.Transaction.Timestamp,
                    TransactionId = at.Transaction.Id,
                    TransactionType = EF.Property<string>(at.Transaction, "TransactionType")
                })
                .ToListAsync();
            return results;
        }
        public async Task<List<TransactionDetails>> GetSourceAccountTransactions(Guid id)
        {
            var results = await _context.AccountsTransactions
                .Include(at => at.Transaction)
                .Where(at => at.AccountId == id && at.Source)
                .Select(at => new TransactionDetails
                {
                    AccountId = at.Source ? at.AccountId : at.Transaction.AccountsTransactions.Where(at => at.Source).Select(at => at.AccountId).FirstOrDefault(),
                    ReceiverId = at.Source ? at.Transaction.AccountsTransactions.Where(at => !at.Source).Select(at => at.AccountId).FirstOrDefault() : at.AccountId,
                    Amount = at.Transaction.Amount,
                    Currency = at.Transaction.Rate.Currency,
                    TransactionDate = at.Transaction.Timestamp,
                    TransactionId = at.Transaction.Id,
                    TransactionType = EF.Property<string>(at.Transaction, "TransactionType")
                })
                .ToListAsync();
            return results;
        }
        public async Task<List<TransactionDetails>> GetReceiverAccountTransactions(Guid id)
        {
            var results = await _context.AccountsTransactions
                .Include(at => at.Transaction)
                .Where(at => at.AccountId == id && !at.Source)
                .Select(at => new TransactionDetails
                {
                    AccountId = at.Source ? at.AccountId : at.Transaction.AccountsTransactions.Where(at => at.Source).Select(at => at.AccountId).FirstOrDefault(),
                    ReceiverId = at.Source ? at.Transaction.AccountsTransactions.Where(at => !at.Source).Select(at => at.AccountId).FirstOrDefault() : at.AccountId,
                    Amount = at.Transaction.Amount,
                    Currency = at.Transaction.Rate.Currency,
                    TransactionDate = at.Transaction.Timestamp,
                    TransactionId = at.Transaction.Id,
                    TransactionType = EF.Property<string>(at.Transaction, "TransactionType")
                })
                .ToListAsync();
            return results;
        }
    }
}