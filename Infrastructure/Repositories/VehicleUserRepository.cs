using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleUserRepository : IVehicleUserRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleUser> AddAsync(VehicleUser relation)
        {
            _context.VehicleUsers.Add(relation);
            await _context.SaveChangesAsync();
            return relation;
        }

        public async Task DeleteAsync(int userId, Guid vehicleId)
        {
            var entity = await _context.VehicleUsers
                .FirstOrDefaultAsync(vu => vu.UserId == userId && vu.VehicleId == vehicleId);

            if (entity != null)
            {
                _context.VehicleUsers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VehicleUser>> GetAllAsync() =>
            await _context.VehicleUsers
                .Include(vu => vu.User)
                .Include(vu => vu.Vehicle)
                .ToListAsync();

        public async Task<VehicleUser?> GetByIdAsync(int userId, Guid vehicleId) =>
            await _context.VehicleUsers
                .Include(vu => vu.User)
                .Include(vu => vu.Vehicle)
                .FirstOrDefaultAsync(vu => vu.UserId == userId && vu.VehicleId == vehicleId);
    }
}
