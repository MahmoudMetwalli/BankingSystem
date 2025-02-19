using System.Security.Principal;
using System.Text.Json;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.AcountsTransactions;
using BankingSystemAPIs.Entities.Rates;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemAPIs.Controllers
{
    /// <summary>
    /// Main controller for account-related operations.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly AccountService _accountService = null!;
        private readonly TransactionService _transactionService = null!;
        private readonly RateService _rateService = null!;
        private readonly AccountTransactionService _accountTransactionService = null!;
        public MainController(
            AccountService accountService,
            TransactionService transactionService,
            RateService rateService,
            AccountTransactionService accountTransactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _rateService = rateService;
            _accountTransactionService = accountTransactionService;
        }
        /// <summary>
        /// Retrieves the balance of a specific account.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <returns>The account balance.</returns>
        [HttpGet("accounts/{id:guid}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetBalance(Guid id)
        {

            Account account = await _accountService.Get(id);
            decimal balance = account.GetBalance();
            var response = new
            {
                Balance = balance
            };
            return Ok(response);
        }
        /// <summary>
        /// Deposits a specified amount into an account.
        /// </summary>
        /// <param name="request">The deposit request details.</param>
        /// <returns>The updated account balance after deposit.</returns>
        [HttpPost("accounts/deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deposit(DepositRequest request)
        {

            Rate rate = await _rateService.Get(request.RateId);
            Account account = await _accountService.Get(request.AccountId);
            decimal depositAmount = _rateService.Convert(request.Amount, rate.CurrencyRate, account.Rate.CurrencyRate);
            await _accountService.Deposit(request.AccountId, depositAmount);
            var deposit = await _transactionService.Add(new Deposit
            (
                request.Amount,
                request.RateId
            ));
            await _accountTransactionService.Add(new AccountTransaction
            (
                request.AccountId,
                deposit.Id,
                true
            ));
            Account newAccount = await _accountService.Get(request.AccountId);
            var response = new
            {
                accountId = request.AccountId,
                DepositAmount = request.Amount,
                accountBalance = account.Balance,
                NewBalance = newAccount.Balance
            };

            return Ok(response);
        }
        /// <summary>
        /// Withdraws a specified amount from an account.
        /// </summary>
        /// <param name="request">The withdrawal request details.</param>
        /// <returns>The updated account balance after withdrawal.</returns>
        [HttpPost("accounts/withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Withdraw(WithdrawRequest request)
        {

            Rate rate = await _rateService.Get(request.RateId);
            Account account = await _accountService.Get(request.AccountId);
            decimal depositAmount = _rateService.Convert(request.Amount, rate.CurrencyRate, account.Rate.CurrencyRate);
            await _accountService.Withdraw(request.AccountId, request.Amount);
            var withdraw = await _transactionService.Add(new Withdraw
            (
                request.Amount,
                request.RateId
            ));
            await _accountTransactionService.Add(new AccountTransaction
            (
                request.AccountId,
                withdraw.Id,
                true
            ));
            Account newAccount = await _accountService.Get(request.AccountId);
            var response = new
            {
                AccountId = request.AccountId,
                WithdrawalAmount = request.Amount,
                Currency = rate.Currency,
                NewBalance = newAccount.Balance
            };

            return Ok(response);
        }
        /// <summary>
        /// Transfers a specified amount from one account to another.
        /// </summary>
        /// <param name="request">The transfer request details.</param>
        /// <returns>The updated balances of the source and target accounts.</returns>
        [HttpPost("accounts/transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Transfer(TransferRequest request)
        {

            Account sourceAccount = await _accountService.Get(request.AccountId);
            Rate rate = await _rateService.Get(request.RateId);
            decimal transferAmountInRelationToSource = _rateService.Convert(
                request.Amount, rate.CurrencyRate, sourceAccount.Rate.CurrencyRate);
            await _accountService.Transfer(
                request.AccountId,
                request.TargetAccountId,
                transferAmountInRelationToSource);
            var transfer = await _transactionService.Add(new Transfer
            (
                request.Amount,
                request.RateId
            ));
            await _accountTransactionService.Add(new AccountTransaction
            (
                request.AccountId,
                transfer.Id,
                true
            ));
            await _accountTransactionService.Add(new AccountTransaction
            (
                request.TargetAccountId,
                transfer.Id,
                false
            ));
            Account newSourceAccount = await _accountService.Get(request.AccountId);
            Account newTargetAccount = await _accountService.Get(request.TargetAccountId);
            var response = new
            {
                AccountId = request.AccountId,
                TransferAmount = request.Amount,
                TargetID = request.TargetAccountId,
                AccountBalance = newSourceAccount.Balance,
                TargetAccountBalance = newTargetAccount.Balance,
                Currency = rate.Currency
            };

            return Ok(response);
        }
    }
}
