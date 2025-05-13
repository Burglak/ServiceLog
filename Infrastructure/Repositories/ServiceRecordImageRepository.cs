using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class ServiceRecordImageRepository : IServiceRecordImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRecordImage> AddAsync(ServiceRecordImage image)
        {
            _context.ServiceRecordImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _context.ServiceRecordImages.FindAsync(id);
            if (image != null)
            {
                _context.ServiceRecordImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ServiceRecordImage>> GetAllAsync() =>
            await _context.ServiceRecordImages.ToListAsync();

        public async Task<ServiceRecordImage?> GetByIdAsync(int id) =>
            await _context.ServiceRecordImages.FindAsync(id);

        public async Task<ServiceRecordImage> UpdateAsync(ServiceRecordImage image)
        {
            _context.ServiceRecordImages.Update(image);
            await _context.SaveChangesAsync();
            return image;
        }
    }
}
