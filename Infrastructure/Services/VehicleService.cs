using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Application.Interfaces;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            // Tworzymy pojazd
            var vehicle = new Vehicle
            {
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                VehicleType = request.VehicleType,
                Vin = request.Vin,
                RegistrationNumber = request.RegistrationNumber,
                EngineType = request.EngineType,
                EngineCapacity = request.EngineCapacity,
                Power = request.Power,
                Mileage = request.Mileage,
                FirstRegistration = request.FirstRegistration,
                Color = request.Color
            };

            var vehicleUser = new VehicleUser
            {
                UserId = userId.Value,
                Vehicle = vehicle
            };

            _context.Vehicles.Add(vehicle);
            _context.VehicleUsers.Add(vehicleUser);

            await _context.SaveChangesAsync();

            return vehicle;
        }

        private int? GetUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : null;
        }
    }
}
