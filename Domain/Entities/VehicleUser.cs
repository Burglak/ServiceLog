using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class VehicleUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid VehicleId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
        [ForeignKey("VehicleId")]
        public required Vehicle Vehicle { get; set; }
    }
}
