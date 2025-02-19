using System;
using System.Security.Principal;
using Amazon.IdentityManagement.Model;
using BankingSystemAPIs.Controllers;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using BankingSystemAPIs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankingSystemAPIsTests
{
    // Test class for unit tests
    public class Tests : IDisposable
    {
        // Private fields for database context options and services
        private DbContextOptions<DataContext> _options;
        private DataContext _context;
        private ClientService _clientService;
        private AccountService _accountService;
        private TransactionService _transactionService;
        private RateService _rateService;
        private AccountTransactionService _accountTransactionService;
        private ClientRepository _clientRepository;
        private RateRepository _rateRepository;
        private AccountTransactionRepository _accountTransactionRepository;
        private TransactionRepository _transactionRepository;
        private AccountRepository _accountRepository;
        private string connectionString;
        private IConfiguration configuration;

        // Setup method to initialize the test environment
        [SetUp]
        public void Setup()
        {
            // Build configuration from appsettings.json
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Get connection string for the test database
            connectionString = configuration.GetConnectionString(
                "TestDatabase") ?? throw new InvalidOperationException("Connection string 'TestDatabase' not found.");

            // Configure DbContext options
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(connectionString).Options;

            // Initialize DataContext and migrate database
            _context = new DataContext(_options);
            _context.Database.Migrate();

            // Initialize repositories and services
            _clientRepository = new ClientRepository(_context);
            _rateRepository = new RateRepository(_context);
            _accountTransactionRepository = new AccountTransactionRepository(_context);
            _transactionRepository = new TransactionRepository(_context);
            _accountRepository = new AccountRepository(_context);
            _accountService = new AccountService(_accountRepository);
            _clientService = new ClientService(_clientRepository);
            _transactionService = new TransactionService(_transactionRepository);
            _rateService = new RateService(_rateRepository);
            _accountTransactionService = new AccountTransactionService(_accountTransactionRepository);
        }

        // Test method to verify client creation
        [Test]
        public async Task CreateClient_Works()
        {
            Client client = new Client
            {
                Name = "Mohamed",
                Email = "Mohamed@email.com"
            };
            var result = await _clientService.Add(client);
            Assert.That(result.Name, Is.EqualTo("Mohamed"));
            await _clientService.Delete(result.Id);
        }

        // Test method to verify client update
        [Test]
        public async Task UpdateClient_Works()
        {
            Client client = new Client
            {
                Name = "Luffy",
                Email = "Noah@email.com"
            };
            var result = await _clientService.Add(client);
            Assert.That(result.Name, Is.EqualTo("Luffy"));
            Assert.That(result.Email, Is.EqualTo("Noah@email.com"));
            Client newClient = new Client
            {
                Name = "Noah",
                Email = "Noah@email.com"
            };
            var newResult = await _clientService.Update(client.Id, newClient);
            Assert.That(newResult.Name, Is.EqualTo("Noah"));
            Assert.That(newResult.Email, Is.EqualTo("Noah@email.com"));
            await _clientService.Delete(newResult.Id);
        }

        // Test method to verify client creation failure
        [Test]
        public async Task CreateClient_Fails()
        {
            Client client = new Client
            {
                Name = "Ashe",
                Email = "Ashe@email.com"
            };
            var result = await _clientService.Add(client);
            Assert.ThrowsAsync<DuplicatedIdException>(() => _clientService.Add(client));
            await _clientService.Delete(result.Id);
        }

        // Test method to verify rate creation
        [Test]
        public async Task CreateRate_Works()
        {
            Rate rate = new Rate
            {
                Currency = "USD",
                CurrencyRate = 1
            };
            var result = await _rateService.Add(rate);
            Assert.That(result.Currency, Is.EqualTo("USD"));
            await _rateService.Delete(result.Id);
        }

        // Test method to verify rate update
        [Test]
        public async Task UpdateRate_Works()
        {
            Rate rate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var result = await _rateService.Add(rate);
            Assert.That(result.CurrencyRate, Is.EqualTo(48));
            Rate newRate = new Rate
            {
                CurrencyRate = 50
            };
            var newResult = await _rateService.Update(rate.Id, newRate);
            Assert.That(newResult.CurrencyRate, Is.EqualTo(50));
            await _rateService.Delete(newResult.Id);
        }

        // Test method to verify rate creation failure
        [Test]
        public async Task CreateRate_Fails()
        {
            Rate rate = new Rate
            {
                Currency = "USD",
                CurrencyRate = 1
            };
            var result = await _rateService.Add(rate);
            Assert.ThrowsAsync<DuplicatedIdException>(() => _rateService.Add(rate));
            await _rateService.Delete(result.Id);
        }

        // Test method to verify account creation
        [Test]
        public async Task AccountCreate_Works()
        {
            Client client = new Client
            {
                Name = "Ahmed",
                Email = "Ahmed@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Ahmed"));
            Rate rate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var rateResult = await _rateService.Add(rate);
            Assert.That(rateResult.Currency, Is.EqualTo("EGP"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            CheckingAccount account = new CheckingAccount(1000, accountNumber, clientResult.Id, rateResult.Id, 400);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            await _accountService.Delete(newAccount.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(rateResult.Id);
        }

        // Test method to verify account update
        [Test]
        public async Task AccountUpdate_Works()
        {
            Client client = new Client
            {
                Name = "Samantha",
                Email = "Samantha@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Samantha"));
            Rate rate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var rateResult = await _rateService.Add(rate);
            Assert.That(rateResult.Currency, Is.EqualTo("EGP"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            CheckingAccount account = new CheckingAccount(1000, accountNumber, clientResult.Id, rateResult.Id, 400);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            CheckingAccount updatedAccount = new CheckingAccount(1000, accountNumber, clientResult.Id, rateResult.Id, 500);
            var result = await _accountService.Update(newAccount.Id, updatedAccount);
            Assert.That(result.AccountNumber, Is.EqualTo(accountNumber));
            await _accountService.Delete(result.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(rateResult.Id);
        }

        // Test method to verify account creation failure
        [Test]
        public async Task AccountCreate_Fails()
        {
            Client client = new Client
            {
                Name = "Donny",
                Email = "Donny@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Donny"));
            Rate rate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var rateResult = await _rateService.Add(rate);
            Assert.That(rateResult.Currency, Is.EqualTo("EGP"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            CheckingAccount account = new CheckingAccount(1000, accountNumber, clientResult.Id, rateResult.Id, 400);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            CheckingAccount updatedAccount = new CheckingAccount(1000, accountNumber, clientResult.Id, rateResult.Id, 500);
            Assert.ThrowsAsync<AccountNumberException>(() => _accountService.Add(updatedAccount));
            await _accountService.Delete(newAccount.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(rateResult.Id);
        }

        // Test method to verify deposit transaction
        [Test]
        public async Task DepositTransaction_Works()
        {
            Client client = new Client
            {
                Name = "Well",
                Email = "Well@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Well"));
            Rate EGPRate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var EGPRateResult = await _rateService.Add(EGPRate);
            Assert.That(EGPRateResult.Currency, Is.EqualTo("EGP"));
            Rate USDRate = new Rate
            {
                Currency = "USD",
                CurrencyRate = 1
            };
            var USDRateResult = await _rateService.Add(USDRate);
            Assert.That(USDRateResult.Currency, Is.EqualTo("USD"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            decimal depostAmount = _rateService.Convert(500, USDRateResult.CurrencyRate, EGPRateResult.CurrencyRate);
            SavingsAccount account = new SavingsAccount(1000, accountNumber, clientResult.Id, EGPRateResult.Id, 3);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            await _accountService.Deposit(newAccount.Id, depostAmount);
            var result = await _accountService.Get(newAccount.Id);
            Assert.That(result.Balance, Is.EqualTo(25000));
            var deposit = await _transactionService.Add(new Deposit
            (500, USDRateResult.Id));
            await _accountTransactionService.Add(new AccountTransaction(newAccount.Id, deposit.Id, true));
            var accountTransactions = await _accountTransactionService.GetAllAccountTransactions(newAccount.Id);
            Assert.That(accountTransactions.Count(), Is.EqualTo(1));
            Assert.That(accountTransactions[0].Amount, Is.EqualTo(500));
            await _transactionService.Delete(accountTransactions[0].TransactionId);
            await _accountService.Delete(newAccount.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(EGPRateResult.Id);
            await _rateService.Delete(USDRateResult.Id);
        }

        // Test method to verify withdraw transaction
        [Test]
        public async Task WithdrawTransaction_Works()
        {
            Client client = new Client
            {
                Name = "Natali",
                Email = "Jonh@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Natali"));
            Rate EGPRate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var EGPRateResult = await _rateService.Add(EGPRate);
            Assert.That(EGPRateResult.Currency, Is.EqualTo("EGP"));
            Rate USDRate = new Rate
            {
                Currency = "USD",
                CurrencyRate = 1
            };
            var USDRateResult = await _rateService.Add(USDRate);
            Assert.That(USDRateResult.Currency, Is.EqualTo("USD"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            decimal withdrawAmount = _rateService.Convert(2400, EGPRateResult.CurrencyRate, USDRateResult.CurrencyRate);
            SavingsAccount account = new SavingsAccount(1000, accountNumber, clientResult.Id, USDRateResult.Id, 3);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            await _accountService.Withdraw(newAccount.Id, withdrawAmount);
            var result = await _accountService.Get(newAccount.Id);
            Assert.That(result.Balance, Is.EqualTo(950));
            var withdraw = await _transactionService.Add(new Withdraw
            (2400, EGPRateResult.Id));
            await _accountTransactionService.Add(new AccountTransaction(newAccount.Id, withdraw.Id, true));
            var accountTransactions = await _accountTransactionService.GetAllAccountTransactions(newAccount.Id);
            Assert.That(accountTransactions.Count(), Is.EqualTo(1));
            Assert.That(accountTransactions[0].Amount, Is.EqualTo(2400));
            await _transactionService.Delete(accountTransactions[0].TransactionId);
            await _accountService.Delete(newAccount.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(EGPRateResult.Id);
            await _rateService.Delete(USDRateResult.Id);
        }

        // Test method to verify transfer transaction
        [Test]
        public async Task TransferTransaction_Works()
        {
            Client client = new Client
            {
                Name = "Doaa",
                Email = "Doaa@email.com"
            };
            var clientResult = await _clientService.Add(client);
            Assert.That(clientResult.Name, Is.EqualTo("Doaa"));
            Rate EGPRate = new Rate
            {
                Currency = "EGP",
                CurrencyRate = 48
            };
            var EGPRateResult = await _rateService.Add(EGPRate);
            Assert.That(EGPRateResult.Currency, Is.EqualTo("EGP"));
            Rate USDRate = new Rate
            {
                Currency = "USD",
                CurrencyRate = 1
            };
            var USDRateResult = await _rateService.Add(USDRate);
            Assert.That(USDRateResult.Currency, Is.EqualTo("USD"));
            Random random = new Random();
            int accountNumber = random.Next(1000, 9999);
            decimal transferAmount = _rateService.Convert(2400, EGPRateResult.CurrencyRate, USDRateResult.CurrencyRate);
            SavingsAccount account = new SavingsAccount(1000, accountNumber, clientResult.Id, USDRateResult.Id, 3);
            var newAccount = await _accountService.Add(account);
            Assert.That(newAccount.AccountNumber, Is.EqualTo(accountNumber));
            int newAccountNumber = random.Next(1000, 9999);
            CheckingAccount checkingAccount = new CheckingAccount(1000, newAccountNumber, clientResult.Id, USDRateResult.Id, 500);
            var newcheckingAccount = await _accountService.Add(checkingAccount);
            Assert.That(newcheckingAccount.AccountNumber, Is.EqualTo(newAccountNumber));
            await _accountService.Transfer(newAccount.Id, newcheckingAccount.Id, transferAmount);
            var result = await _accountService.Get(newAccount.Id);
            Assert.That(result.Balance, Is.EqualTo(950));
            var receiverResult = await _accountService.Get(newcheckingAccount.Id);
            Assert.That(receiverResult.Balance, Is.EqualTo(1050));
            var transfer = await _transactionService.Add(new Transfer
            (2400, EGPRateResult.Id));
            await _accountTransactionService.Add(new AccountTransaction(newAccount.Id, transfer.Id, true));
            await _accountTransactionService.Add(new AccountTransaction(newcheckingAccount.Id, transfer.Id, false));
            var accountTransactions = await _accountTransactionService.GetSourceAccountTransactions(newAccount.Id);
            Assert.That(accountTransactions.Count(), Is.EqualTo(1));
            var checkingAccountTransactions = await _accountTransactionService.GetReceiverAccountTransactions(newcheckingAccount.Id);
            Assert.That(checkingAccountTransactions.Count(), Is.EqualTo(1));
            Assert.That(checkingAccountTransactions[0].TransactionId, Is.EqualTo(accountTransactions[0].TransactionId));
            await _transactionService.Delete(accountTransactions[0].TransactionId);
            await _accountService.Delete(newAccount.Id);
            await _accountService.Delete(newcheckingAccount.Id);
            await _clientService.Delete(clientResult.Id);
            await _rateService.Delete(EGPRateResult.Id);
            await _rateService.Delete(USDRateResult.Id);
        }

        // Test method to verify account deposit
        [Test]
        public void AccountDepost_Works()
        {
            Random random = new Random();
            SavingsAccount account = new SavingsAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 0.02m);
            account.Deposit(1000);

            Assert.That(account.Balance, Is.EqualTo(2000));
        }

        // Test method to verify savings account withdrawal
        [Test]
        public void AccountWithdrawSavingsAccount_Works()
        {
            Random random = new Random();
            SavingsAccount account = new SavingsAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 0.02m);
            account.Withdraw(1000);

            Assert.That(account.Balance, Is.EqualTo(0));
        }

        // Test method to verify savings account withdrawal failure
        [Test]
        public void AccountWithdrawSavingsAccount_Fails()
        {
            Random random = new Random();
            SavingsAccount account = new SavingsAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 0.02m);

            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(1500));
        }

        // Test method to verify checking account withdrawal
        [Test]
        public void AccountWithdrawCheckingAccount_Works()
        {
            Random random = new Random();
            CheckingAccount account = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);
            account.Withdraw(1400);

            Assert.That(account.Balance, Is.EqualTo(-400));
        }
        // Test method to verify checking account withdrawal failure when the amount exceeds the balance and overdraft limit
        [Test]
        public void AccountWithdrawCheckingAccount_Fails()
        {
            Random random = new Random();
            CheckingAccount account = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);

            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(1500));
        }
        // Test method to verify Transfer Success between two accounts
        [Test]
        public void AccountTransfer_Works()
        {
            Random random = new Random();
            CheckingAccount accountA = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);
            CheckingAccount accountB = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);

            accountA.Transfer(accountB, 10, 1, 50);
            Assert.That(accountA.Balance, Is.EqualTo(990));
            Assert.That(accountB.Balance, Is.EqualTo(1500));
        }
        // Test method to verify Transfer Failure when the amount exceeds the balance
        [Test]
        public void AccountTransfer_Fails()
        {
            Random random = new Random();
            CheckingAccount accountA = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);
            CheckingAccount accountB = new CheckingAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 400);

            Assert.Throws<InsufficientFundsException>(() => accountA.Transfer(accountB, 1100, 1, 50));
        }
        // Test method to verify interest calculation and addition to a savings account
        [Test]
        public void AccountInterest_Works()
        {
            Random random = new Random();
            SavingsAccount accountA = new SavingsAccount(1000, random.Next(1000, 9999), Guid.NewGuid(), Guid.NewGuid(), 5);
            Assert.That(accountA.InterestCalculation(2), Is.EqualTo(102.5));
            accountA.AddInterest(3);
            Assert.That(accountA.Balance, Is.EqualTo(1157.625));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}