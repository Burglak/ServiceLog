using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleRepository : Repository<Vehicle>
    {
        public VehicleRepository(DbContext context) : base(context) { }

        public async Task<Vehicle?> GetWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(v => v.VehicleUsers).ThenInclude(vu => vu.User)
                .Include(v => v.VehicleImages)
                .Include(v => v.Notifications)
                .Include(v => v.ServiceRecords).ThenInclude(sr => sr.serviceRecordImages)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
