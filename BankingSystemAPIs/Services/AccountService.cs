using System.Security.Principal;
using Amazon.Runtime.Internal;
using Amazon.SimpleEmail.Model;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace BankingSystemAPIs.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository = null!;

        // Constructor to inject DataContext
        public AccountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // Retrieve a specific account by its ID
        public async Task<Account> Get(Guid id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        // Get a list of all accounts
        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _accountRepository.GetAllAsync();
        }
        public async Task<IEnumerable<SavingsAccount>> GetSavingsAccountsAll()
        {
            return await _accountRepository.GetAllSavingsAccountsAsync();
        }
        public async Task<IEnumerable<CheckingAccount>> GetCheckingAccountsAll()
        {
            return await _accountRepository.GetAllCheckingAccountsAsync();
        }

        // Add a new account, ensuring the account number is unique
        public async Task<Account> Add(Account account)
        {
            return await _accountRepository.AddAsync(account);
        }

        // Update an existing account by its ID, ensuring the account number is unique
        public async Task<Account> Update(Guid id, Account account)
        {
            return await _accountRepository.UpdateAsync(id, account);
        }

        // Delete an account by its ID
        public async Task Delete(Guid id)
        {
            await _accountRepository.DeleteAsync(id);
        }

        // Deposit an amount into an account
        public async Task Deposit(Guid id, decimal amount)
        {
            await _accountRepository.ExecuteInTransactionAsync(async () =>
            {
                Account account = await Get(id);
                account.Deposit(amount);
            });
        }

        // Withdraw an amount from an account
        public async Task Withdraw(Guid id, decimal amount)
        {
            await _accountRepository.ExecuteInTransactionAsync(async () =>
            {
                Account account = await Get(id);
                account.Withdraw(amount);
            });
        }

        // Transfer funds between two accounts
        public async Task Transfer(Guid id, Guid targetId, decimal amount)
        {
            await _accountRepository.ExecuteInTransactionAsync(async () =>
            {
                Account account = await Get(id);
                Account targetAccount = await Get(targetId);
                account.Transfer(targetAccount, amount, account.Rate.CurrencyRate, targetAccount.Rate.CurrencyRate);
            });
        }

        public async Task<SavingsAccount> GetSavingsAccount(Guid id)
        {
            Account account = await Get(id);
            if (account is not SavingsAccount)
                throw new AccountTypeException("Account is not a Savings account.");
            return (SavingsAccount)account;
        }

        // Add interest to a savings account
        public async Task AddInterest(Guid id, int years)
        {
            await _accountRepository.ExecuteInTransactionAsync(async () =>
            {
                SavingsAccount account = await GetSavingsAccount(id);
                account.AddInterest(years);
            }); 
        }

        // Calculate the interest for a savings account
        public async Task<decimal> InterestCalculation(Guid id, int years)
        {
            SavingsAccount account = await GetSavingsAccount(id);
            return account.InterestCalculation(years);
        }
    }
}
