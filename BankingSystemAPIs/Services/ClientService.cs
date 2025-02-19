using System.Net.Sockets;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.Accounts;
using BankingSystemAPIs.Entities.Transactions;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using BankingSystemAPIs.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Services
{
    public class ClientService
    {
        // Database context for interacting with the database
        private readonly ClientRepository _clientRepository = null!;

        // Constructor to initialize the database context via dependency injection
        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Retrieves a single client by ID
        public async Task<Client> Get(Guid id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }

        // Retrieves all clients from the Clients table
        public async Task<IEnumerable<Client>> GetAll()
        {
            // Return a list of all clients in the database
            return await _clientRepository.GetAllAsync();
        }

        // Adds a new client to the Clients table
        public async Task<Client> Add(Client client)
        {

            return await _clientRepository.AddAsync(client);
        }
  
        // Updates an existing client
        public async Task<Client> Update(Guid id, Client client)
        {
            return await _clientRepository.UpdateAsync(id, client);
        }

        // Deletes a client by ID
        public async Task Delete(Guid id)
        {
            await _clientRepository.DeleteAsync(id);
        }
    }
}
