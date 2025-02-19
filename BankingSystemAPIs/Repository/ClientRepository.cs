using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Repository
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly DataContext _context = null!;

        // Constructor to initialize the database context via dependency injection
        public ClientRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Client> AddAsync(Client client)
        {
            Client? clientCheck = await _context.Clients
                .Where(c => c.Name == client.Name) // Look for an existing client with the same name
                .Select(client => client).FirstOrDefaultAsync();
             
            // If no duplicate client is found, add the new client
            if (clientCheck == null)
            {
                await _context.Clients.AddAsync(client); // Add the client to the database
                await _context.SaveChangesAsync(); // Commit the changes
                return client; // Return the added client
            }
            else
            {
                // If a duplicate is found, throw an exception
                throw new DuplicatedIdException("Duplicated Client Name");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the client to delete by ID
                Client tempClient = await GetByIdAsync(id);

                // Remove the client from the Clients table
                _context.Clients.Remove(tempClient);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to apply the deletion
                await dBtransaction.CommitAsync();
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            // Attempt to find the client by ID
            Client? client = await _context.Clients.FindAsync(id);

            // If the client is not found, throw a custom exception
            return client == null ? throw new ClientNotFoundException("Client is not found") : client;
        }

        public async Task<Client> UpdateAsync(Guid id, Client client)
        {
            // Begin a database transaction to ensure atomicity
            using var dBtransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the existing client by ID
                Client tempUser = await GetByIdAsync(id);

                // Ensure the client ID is preserved during the update
                client.Id = tempUser.Id;

                // Detach the original client entity to avoid conflict during the update
                _context.Entry(tempUser).State = EntityState.Detached;

                // Mark the new client entity as modified
                _context.Entry(client).State = EntityState.Modified;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction to apply the changes
                await dBtransaction.CommitAsync();

                // Return the updated client
                return client;
            }
            catch
            {
                // If any error occurs, roll back the transaction to maintain consistency
                await dBtransaction.RollbackAsync();
                throw;
            }
        }
    }
}
