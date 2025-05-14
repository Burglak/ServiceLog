using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Seed
{
    [SeederOrder(3)]
    public class VehicleUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.VehicleUsers.Any()) return;

            var links = new[]
            {
                new VehicleUser { UserId = 1, VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new VehicleUser { UserId = 2, VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new VehicleUser { UserId = 3, VehicleId = Guid.Parse("33333333-3333-3333-3333-333333333333") },
                new VehicleUser { UserId = 4, VehicleId = Guid.Parse("44444444-4444-4444-4444-444444444444") },
                new VehicleUser { UserId = 5, VehicleId = Guid.Parse("55555555-5555-5555-5555-555555555555") }
            };

            context.VehicleUsers.AddRange(links);
            await context.SaveChangesAsync();
        }
    }
}
