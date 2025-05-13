using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleUserRepository : Repository<VehicleUser>
    {
        public VehicleUserRepository(DbContext context) : base(context) { }
    }
}
