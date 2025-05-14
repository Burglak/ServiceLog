using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task DeleteAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync() => await _context.Vehicles.ToListAsync();

        public async Task<Vehicle?> GetByIdAsync(Guid id) => await _context.Vehicles.FindAsync(id);

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> GetWithDetailsAsync(Guid id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetFilteredAsync(VehicleFilterRequest f)
        {
            IQueryable<Vehicle> q = _context.Vehicles;

            if (!string.IsNullOrWhiteSpace(f.Make))
                q = q.Where(v => v.Make.Contains(f.Make));
            if (!string.IsNullOrWhiteSpace(f.Model))
                q = q.Where(v => v.Model.Contains(f.Model));
            if (f.MinEngineCapacity.HasValue)
                q = q.Where(v => v.EngineCapacity >= f.MinEngineCapacity.Value);
            if (f.MaxEngineCapacity.HasValue)
                q = q.Where(v => v.EngineCapacity <= f.MaxEngineCapacity.Value);
            if (f.MinPower.HasValue)
                q = q.Where(v => v.Power >= f.MinPower.Value);
            if (f.MaxPower.HasValue)
                q = q.Where(v => v.Power <= f.MaxPower.Value);
            if (!string.IsNullOrWhiteSpace(f.Vin))
                q = q.Where(v => v.Vin != null && v.Vin.Contains(f.Vin));

            return await q.ToListAsync();
        }
    }
}
