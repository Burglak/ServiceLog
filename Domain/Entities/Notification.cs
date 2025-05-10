using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class Notification : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime NotifyAt { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
