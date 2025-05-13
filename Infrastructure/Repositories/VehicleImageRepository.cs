using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class VehicleImageRepository : Repository<VehicleImage>
    {
        public VehicleImageRepository(DbContext context) : base(context) { }
    }
}
