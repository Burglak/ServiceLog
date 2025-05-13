using ServiceLog.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Application.DTOs.Vehicle
{
    public class CreateVehicleRequest
    {
        [Required] public string Make { get; set; } = string.Empty;
        [Required] public string Model { get; set; } = string.Empty;
        [Required] public VehicleType VehicleType { get; set; }
        public int Year { get; set; }
        public string? Vin { get; set; }
        public string? RegistrationNumber { get; set; }
        public EngineType EngineType { get; set; }
        public decimal EngineCapacity { get; set; }
        public int Power { get; set; }
        public int Mileage { get; set; }
        public DateTime FirstRegistration { get; set; }
        public VehicleColor Color { get; set; }
    }
}
