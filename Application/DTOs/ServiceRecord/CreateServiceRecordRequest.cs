using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Application.DTOs.ServiceRecord
{
    public class CreateServiceRecordRequest
    {
        [Required] public Guid VehicleId { get; set; }
        [Required] public DateTime ServiceDate { get; set; }
        [Required] public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0, int.MaxValue)] public int MileageAt { get; set; }
        [Range(0, double.MaxValue)] public decimal Cost { get; set; }
        public string? WorkshopName { get; set; }
    }
}
