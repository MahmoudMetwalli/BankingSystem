using BankingSystemAPIs.Entities.Accounts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Entities.User
{
    // The Client class represents a user in the banking system, containing details like Name, Email, and associated Accounts.
    // An index is created on the Name property to enforce uniqueness, ensuring no two clients have the same name.
    [Index(nameof(Name), IsUnique = true)]
    public class Client
    {
        // Unique identifier for the client. This is a GUID that is automatically generated.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // Client's name. This is required and can be up to 100 characters.
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Client's email address. This is required and validated using an email address format.
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // Default constructor, which allows creating a client without setting properties initially.
        public Client() { }

        // Constructor to initialize a client with a name and email.
        public Client(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
