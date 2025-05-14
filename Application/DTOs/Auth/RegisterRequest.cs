using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
