using Microsoft.AspNetCore.Mvc;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.Rates;
using Microsoft.Extensions.Logging;
using BankingSystemAPIs.Services;
using BankingSystemAPIs.Migrations;
using BankingSystemAPIs.Dtos;

namespace BankingSystemAPIs.Controllers
{
    /// <summary>
    /// Controller for managing currency exchange rates.
    /// </summary>
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly RateService _rateService = null!;

        public RateController(RateService rateService)
        {
            _rateService = rateService;
        }

        /// <summary>
        /// Gets the exchange rate by currency code.
        /// </summary>
        /// <param name="currency">The currency code.</param>
        /// <returns>The exchange rate.</returns>
        [HttpGet("{currency}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetExchangeRate(string currency)
        {

            var rate = await _rateService.GetExchangeRate(currency);

            return Ok(rate);
        }
        /// <summary>
        /// Gets the rate by Id.
        /// </summary>
        /// <param name="id">The currency Id.</param>
        /// <returns>The exchange rate.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetRate(Guid id)
        {

            var rate = await _rateService.Get(id);

            return Ok(rate);
        }
        /// <summary>
        /// Gets all exchange rates.
        /// </summary>
        /// <returns>A list of all exchange rates.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetAllRates()
        {
            var rates = await _rateService.GetAll();

            return Ok(new { Rates = rates });
        }

        /// <summary>
        /// Creates a new exchange rate.
        /// </summary>
        /// <param name="rate">The rate to create.</param>
        /// <returns>The created rate.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRate(RateRequest rateRequest)
        {

            var createdRate = await _rateService.Add(new Rate(rateRequest.Currency, rateRequest.CurrencyRate));

            return CreatedAtAction(nameof(GetRate), new { id = createdRate.Id }, createdRate);
        }

        /// <summary>
        /// Updates an existing exchange rate.
        /// </summary>
        /// <param name="id">The currency Id of the rate to update.</param>
        /// <param name="rate">The updated rate details.</param>
        /// <returns>The updated rate.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRate(Guid id, Rate rate)
        {
            var updatedRate = await _rateService.Update(id, rate);

            return Ok(updatedRate);
        }

        /// <summary>
        /// Deletes an exchange rate.
        /// </summary>
        /// <param name="id">The currency Id of the rate to delete.</param>
        /// <returns>A confirmation of the deletion.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRate(Guid id)
        {

            await _rateService.Delete(id);
            var response = new { Deleted = "Success" };
            return Ok(response);
        }
    }
}
