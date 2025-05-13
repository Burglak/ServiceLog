using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Application.Interfaces;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
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

        // Pobieranie UserId z tokenu JWT
        private int? GetUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : null;
        }

        // Tworzenie nowego pojazdu
        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

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

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Przypisanie pojazdu do użytkownika
            _context.VehicleUsers.Add(new VehicleUser { VehicleId = vehicle.Id, UserId = userId.Value });
            await _context.SaveChangesAsync();

            return vehicle;
        }

        // Pobieranie pojazdu po ID
        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            return await _context.Vehicles
                .Where(v => v.Id == id && _context.VehicleUsers.Any(vu => vu.VehicleId == v.Id && vu.UserId == userId))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetUserVehiclesAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var vehicleIds = await _context.VehicleUsers
                .Where(vu => vu.UserId == userId)
                .Select(vu => vu.VehicleId)
                .ToListAsync();

            var vehicles = await _context.Vehicles
                .Where(v => vehicleIds.Contains(v.Id))
                .ToListAsync();

            return vehicles;
        }



        // Aktualizacja pojazdu
        public async Task<Vehicle> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var isOwned = await _context.VehicleUsers.AnyAsync(vu => vu.VehicleId == id && vu.UserId == userId);
            if (!isOwned)
                throw new InvalidOperationException("Vehicle not found or does not belong to the user");

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found");

            // Warunkowa aktualizacja właściwości
            if (request.Make != null) vehicle.Make = request.Make;
            if (request.Model != null) vehicle.Model = request.Model;
            if (request.Year.HasValue) vehicle.Year = request.Year.Value;
            if (request.VehicleType.HasValue) vehicle.VehicleType = request.VehicleType.Value;
            if (request.Vin != null) vehicle.Vin = request.Vin;
            if (request.RegistrationNumber != null) vehicle.RegistrationNumber = request.RegistrationNumber;
            if (request.EngineType.HasValue) vehicle.EngineType = request.EngineType.Value;
            if (request.EngineCapacity.HasValue) vehicle.EngineCapacity = request.EngineCapacity.Value;
            if (request.Power.HasValue) vehicle.Power = request.Power.Value;
            if (request.Mileage.HasValue) vehicle.Mileage = request.Mileage.Value;
            if (request.FirstRegistration.HasValue) vehicle.FirstRegistration = request.FirstRegistration.Value;
            if (request.Color.HasValue) vehicle.Color = request.Color.Value;

            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var isOwned = await _context.VehicleUsers.AnyAsync(vu => vu.VehicleId == id && vu.UserId == userId);
            if (!isOwned)
                throw new InvalidOperationException("Vehicle not found or does not belong to the user");

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found");

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

    }
}
