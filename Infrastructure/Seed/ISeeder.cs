using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Seed
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext context);
    }
}
