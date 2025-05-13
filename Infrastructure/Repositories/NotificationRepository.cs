using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>
    {
        public NotificationRepository(DbContext context) : base (context) { }
    }
}
