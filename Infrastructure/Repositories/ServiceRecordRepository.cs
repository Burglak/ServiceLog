using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class ServiceRecordRepository : Repository<ServiceRecord>
    {
        public ServiceRecordRepository(DbContext context) : base(context) { }

        public async Task<ServiceRecord?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(sr => sr.serviceRecordImages)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }
    }
}
