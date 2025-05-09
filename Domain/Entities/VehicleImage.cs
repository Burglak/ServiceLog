using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class VehicleImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid VehicleId { get; set; }
        [Required]
        public required string ImagePath {  get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; } = null!;
    }
}
