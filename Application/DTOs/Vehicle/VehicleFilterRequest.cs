namespace ServiceLog.Application.DTOs.Vehicle
{
    public class VehicleFilterRequest
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public decimal? MinEngineCapacity { get; set; }
        public decimal? MaxEngineCapacity { get; set; }
        public int? MinPower { get; set; }
        public int? MaxPower { get; set; }
        public string? Vin { get; set; }
    }
}
