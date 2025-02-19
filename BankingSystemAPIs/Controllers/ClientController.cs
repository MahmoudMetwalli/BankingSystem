using Microsoft.AspNetCore.Mvc;
using BankingSystemAPIs.Data;
using Microsoft.Extensions.Logging;
using BankingSystemAPIs.Services;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Dtos;

namespace BankingSystemAPIs.Controllers
{
    /// <summary>
    /// Controller for managing Clients.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService = null!;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }
        /// <summary>
        /// Retrieves a client by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <returns>The client details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetClient(Guid id)
        {

            var user = await _clientService.Get(id);

            return Ok(user);
        }
        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        /// <returns>A list of all clients.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Client>))]
        public async Task<ActionResult> GetAllClients()
        {

            var users = await _clientService.GetAll();

            return Ok(new { Users = users });
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="client">The client details.</param>
        /// <returns>The created client.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Client))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateClient(ClientRequest client)
        {

            var createdClient = await _clientService.Add(new Client(client.Name, client.Email));

            return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
        }
        /// <summary>
        /// Updates an existing client by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <param name="client">The updated client details.</param>
        /// <returns>The updated client.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateClient(Guid id, Client client)
        {

            var updatedClient = await _clientService.Update(id, client);

            return Ok(updatedClient);
        }

        /// <summary>
        /// Deletes a client by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <returns>A success message if deletion is successful.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            await _clientService.Delete(id);

            var response = new { Deleted = "Success" };

            return Ok(response);
        }
    }
}
