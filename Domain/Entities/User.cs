using ServiceLog.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public User()
        {
            
        }
    }
}
