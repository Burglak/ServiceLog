using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class ServiceRecord : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        public DateTime ServiceDate { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public int MileageAt { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        public string? WorkshopName { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
