using System.ComponentModel.DataAnnotations;

namespace BmmAPI.DTOs
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set;}
        [Required]
        public string? Password { get; set; }

        public int UserCount { get; set; } // Add this property for user count
    
    }
}
