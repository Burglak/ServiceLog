namespace ServiceLog.Application.DTOs.ServiceRecord
{
    public class UpdateServiceRecordRequest
    {
        public DateTime? ServiceDate { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? MileageAt { get; set; }
        public decimal? Cost { get; set; }
        public string? WorkshopName { get; set; }
    }
}
