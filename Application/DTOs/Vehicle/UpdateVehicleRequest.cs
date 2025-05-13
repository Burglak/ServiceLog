using ServiceLog.Domain.Enums;

namespace ServiceLog.Application.DTOs.Vehicle
{
    public class UpdateVehicleRequest
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public VehicleType? VehicleType { get; set; }
        public string? Vin { get; set; }
        public string? RegistrationNumber { get; set; }
        public EngineType? EngineType { get; set; }
        public decimal? EngineCapacity { get; set; }
        public int? Power { get; set; }
        public int? Mileage { get; set; }
        public DateTime? FirstRegistration { get; set; }
        public VehicleColor? Color { get; set; }
    }
}
