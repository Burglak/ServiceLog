using ServiceLog.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        { 
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
           
        }
    }
}
