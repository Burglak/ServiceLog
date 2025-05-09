using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class ServiceRecord
    {
        [Key]
        public int Id { get; set; }
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
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; } = null!;

        public ServiceRecord()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
