using System.Net.Sockets;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Services
{
    public class AccountTransactionService
    {
        private readonly AccountTransactionRepository _accountTransactionRepository = null!;
        public AccountTransactionService(AccountTransactionRepository accountTransactionRepository)
        {
            _accountTransactionRepository = accountTransactionRepository;
        }
        public async Task<AccountTransaction> Add(AccountTransaction accountTransaction)
        {
            return await _accountTransactionRepository.AddAsync(accountTransaction);
        }
        public async Task<List<TransactionDetails>> GetAllAccountTransactions(Guid id)
        {
            return await _accountTransactionRepository.GetAllAccountTransactions(id);
        }
        public async Task<List<TransactionDetails>> GetSourceAccountTransactions(Guid id)
        {
            return await _accountTransactionRepository.GetSourceAccountTransactions(id);
        }
        public async Task<List<TransactionDetails>> GetReceiverAccountTransactions(Guid id)
        {
            return await _accountTransactionRepository.GetReceiverAccountTransactions(id);
        }
    }
}
