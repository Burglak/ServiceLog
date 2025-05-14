using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Application.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
