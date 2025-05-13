using Microsoft.EntityFrameworkCore;
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
                .Include(v => v.VehicleUsers).ThenInclude(vu => vu.User)
                .Include(v => v.VehicleImages)
                .Include(v => v.Notifications)
                .Include(v => v.ServiceRecords).ThenInclude(sr => sr.serviceRecordImages)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
