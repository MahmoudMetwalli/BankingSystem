using Microsoft.AspNetCore.Mvc;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.Transactions;
using Microsoft.Extensions.Logging;
using BankingSystemAPIs.Services;

namespace BankingSystemAPIs.Controllers
{
    /// <summary>
    /// Controller for managing transactions.
    /// </summary>
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService = null!;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        /// <summary>
        /// Retrieves a transaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTransaction(Guid id)
        {

            var transaction = await _transactionService.Get(id);

            return Ok(transaction);
        }
        /// <summary>
        /// Retrieves all transactions.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllTransactions()
        {

            var transactions = await _transactionService.GetAll();

            return Ok(new { Transactions = transactions });
        }

        /// <summary>
        /// Deletes a transaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the transaction to delete.</param>
        /// <returns>A confirmation of the deletion.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {

            await _transactionService.Delete(id);
            var response = new { Deleted = "Success" };
            return Ok(response);
        }
    }
}
