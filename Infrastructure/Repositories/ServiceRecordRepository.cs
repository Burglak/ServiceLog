using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class ServiceRecordRepository : IServiceRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRecord> AddAsync(ServiceRecord record)
        {
            _context.ServiceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.ServiceRecords.FindAsync(id);
            if (record != null)
            {
                _context.ServiceRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ServiceRecord>> GetAllAsync() =>
            await _context.ServiceRecords.ToListAsync();

        public async Task<ServiceRecord?> GetByIdAsync(int id) =>
            await _context.ServiceRecords.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<ServiceRecord> UpdateAsync(ServiceRecord record)
        {
            _context.ServiceRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }
    }
}
