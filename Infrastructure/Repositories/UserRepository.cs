using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DbContext context) : base(context) { }

        public async Task<User?> GetWithVehicleUsersAsync(int id)
        {
            return await _dbSet
                .Include(u => u.VehicleUsers)
                .ThenInclude(vu => vu.Vehicle)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
