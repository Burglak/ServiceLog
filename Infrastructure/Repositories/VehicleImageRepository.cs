using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleImageRepository : IVehicleImageRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleImage> AddAsync(VehicleImage image)
        {
            _context.VehicleImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _context.VehicleImages.FindAsync(id);
            if (image != null)
            {
                _context.VehicleImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VehicleImage>> GetAllAsync() =>
            await _context.VehicleImages.Include(i => i.Vehicle).ToListAsync();

        public async Task<VehicleImage?> GetByIdAsync(int id) =>
            await _context.VehicleImages.Include(i => i.Vehicle).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<VehicleImage> UpdateAsync(VehicleImage image)
        {
            _context.VehicleImages.Update(image);
            await _context.SaveChangesAsync();
            return image;
        }
    }
}
