using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    [NotMapped]
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
