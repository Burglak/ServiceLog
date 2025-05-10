using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class VehicleUser : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        public User User { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
    }
}
