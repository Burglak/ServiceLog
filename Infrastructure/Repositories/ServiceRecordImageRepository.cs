using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class ServiceRecordImageRepository : Repository<ServiceRecordImage>
    {
        public ServiceRecordImageRepository(DbContext context) : base(context) { }
    }
}
