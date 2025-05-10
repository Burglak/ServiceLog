using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Enums;

namespace ServiceLog.Domain.Entities
{
    [Index(nameof(Vin), IsUnique = true)]
    [Index(nameof(RegistrationNumber), IsUnique = true)]
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required VehicleType VehicleType { get; set; }
        public int Year { get; set; }
        public string? Vin { get; set; }
        public string? RegistrationNumber { get; set; }
        public EngineType EngineType { get; set; }
        [Range(0, 10.0)]
        public decimal EngineCapacity { get; set; }
        [Range(0, 2000.0)]
        public int Power { get; set; }
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }
        public DateTime FirstRegistration { get; set; }
        public VehicleColor Color { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<VehicleUser> VehicleUsers { get; set; } = [];
        public ICollection<VehicleImage> VehicleImages { get; set; } = [];
        public ICollection<Notification> Notifications { get; set; } = [];
        public ICollection<ServiceRecord> ServiceRecords { get; set; } = [];

        public Vehicle()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
