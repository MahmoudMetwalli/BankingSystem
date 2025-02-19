using System.Security.Principal;
using System.Text.Json;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Dtos;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingSystemAPIs.Controllers
{
    /// <summary>
    /// Controller for managing Accounts.
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly AccountTransactionService _accountTransactionsService;

        public AccountController(
            AccountService accountService,
            AccountTransactionService accountTransactionsService)
        {
            _accountService = accountService;
            _accountTransactionsService = accountTransactionsService;
        }


        /// <summary>
        /// Creates a new savings account.
        /// </summary>
        /// <param name="savingAccountData">Details of the savings account.</param>
        /// <returns>The created savings account.</returns>
        [HttpPost("savings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddSavingsAccount(SavingsAccountRequest savingAccountData)
        {

            var addedAccount = await _accountService.Add(new SavingsAccount(
                savingAccountData.Balance,
                savingAccountData.AccountNumber,
                savingAccountData.UserId,
                savingAccountData.RateId,
                savingAccountData.Interest));

            return CreatedAtAction(nameof(GetAccount), new { id = addedAccount.Id }, addedAccount);
        }

        /// <summary>
        /// Creates a new checking account.
        /// </summary>
        /// <param name="checkingAccountData">Details of the checking account.</param>
        /// <returns>The created checking account.</returns>
        [HttpPost("checking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddCheckingAccount(CheckingAccountRequest checkingAccountData)
        {

            var addedAccount = await _accountService.Add(new CheckingAccount(
                checkingAccountData.Balance,
                checkingAccountData.AccountNumber,
                checkingAccountData.UserId,
                checkingAccountData.RateId,
                checkingAccountData.OverDraft));

            return CreatedAtAction(nameof(GetAccount), new { id = addedAccount.Id }, addedAccount);
        }

        /// <summary>
        /// Updates an account by ID.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <param name="account">Updated account details.</param>
        /// <returns>The updated account.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid id, Account account)
        {

            var updatedAccount = await _accountService.Update(id, account);

            return Ok(updatedAccount);
        }
        /// <summary>
        /// Reads an Account
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>The Fetched Account</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAccount(Guid id)
        {

            var account = await _accountService.Get(id);

            return Ok(account);
        }
        /// <summary>
        /// Reads all Accounts
        /// </summary>
        /// <returns>The Fetched Accounts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllAccounts()
        {

            var account = await _accountService.GetAll();

            return Ok(account);
        }
        /// <summary>
        /// Reads Savings Accounts
        /// </summary>
        /// <returns>The Fetched Accounts</returns>
        [HttpGet("savings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllSavingsAccounts()
        {

            var account = await _accountService.GetSavingsAccountsAll();

            return Ok(account);
        }
        /// <summary>
        /// Reads Checking Accounts
        /// </summary>
        /// <returns>The Fetched Accounts</returns>
        [HttpGet("checking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCheckingAccounts()
        {

            var account = await _accountService.GetCheckingAccountsAll();

            return Ok(account);
        }
        /// <summary>
        /// Deletes an account by ID.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>A success message if deleted.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {

            await _accountService.Delete(id);
            var response = new { Deleted = "Success" };

            return Ok(response);
        }

        /// <summary>
        /// Adds interest to a savings account.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <param name="years">Number of years.</param>
        /// <returns>The account with updated balance.</returns>
        [HttpPost("{id:guid}/interest/{years:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddInterest(Guid id, int years)
        {

            await _accountService.AddInterest(id, years);

            return Ok(new { Interest = "Success" });
        }

        /// <summary>
        /// Calculates interest for an account.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <param name="years">Number of years.</param>
        /// <returns>The calculated interest.</returns>
        [HttpGet("{id:guid}/interest-calculation/{years:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> InterestCalculation(Guid id, int years)
        {

            var interest = await _accountService.InterestCalculation(id, years);

            return Ok(new { Interest = interest });
        }

        /// <summary>
        /// Retrieves all transactions for an account.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>A list of transactions.</returns>
        [HttpGet("{id:guid}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllAccountTransactions(Guid id)
        {

            var transactions = await _accountTransactionsService.GetAllAccountTransactions(id);

            return Ok(new { Transactions = transactions });
        }

        /// <summary>
        /// Retrieves source transactions for an account.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>A list of source transactions.</returns>
        [HttpGet("{id:guid}/transactions/source")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSourceAccountTransactions(Guid id)
        {

            var transactions = await _accountTransactionsService.GetSourceAccountTransactions(id);

            return Ok(new { Transactions = transactions });
        }

        /// <summary>
        /// Retrieves target transactions for an account.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>A list of target transactions.</returns>
        [HttpGet("{id:guid}/transactions/target")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetReceiverAccountTransactions(Guid id)
        {

            var transactions = await _accountTransactionsService.GetReceiverAccountTransactions(id);

            return Ok(new { Transactions = transactions });
        }
    }
}
