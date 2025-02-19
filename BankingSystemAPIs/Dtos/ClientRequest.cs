using System;
using System.ComponentModel.DataAnnotations;

namespace BankingSystemAPIs.Dtos
{
    // The ClientRequest class is used to represent the data needed when creating or updating a client.
    // This class ensures that the necessary fields, such as the client's name and email, are provided.
    public class ClientRequest
    {
        // The Name property is required when creating a client. 
        // It has a maximum length constraint of 100 characters.
        // The error message is customized to prompt users to provide a name if it's missing.
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100)] // Name can be at most 100 characters long.
        public string Name { get; set; } = null!;

        // The Email property is required and must be in a valid email format.
        // The EmailAddress attribute ensures that the value conforms to the format of an email.
        // If missing or invalid, an appropriate error message will be shown.
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress] // Ensures the email follows a valid format.
        public string Email { get; set; } = null!;
    }
}
