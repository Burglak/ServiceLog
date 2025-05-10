using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class VehicleImage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        [Required]
        public required string ImagePath {  get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
