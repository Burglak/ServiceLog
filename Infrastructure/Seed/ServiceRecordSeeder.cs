using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace ServiceLog.Infrastructure.Seed
{
    [SeederOrder(4)]
    public class ServiceRecordSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.ServiceRecords.Any()) return;

            var records = new[]
            {
                new ServiceRecord {
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ServiceDate = new DateTime(2021, 6, 1), Title = "Oil Change",
                    Description = "Standard engine oil and filter change", MileageAt = 30000,
                    Cost = 79.99m, WorkshopName = "Speedy Lube"
                },
                new ServiceRecord {
                    VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ServiceDate = new DateTime(2022, 4, 10), Title = "Brake Pads Replacement",
                    Description = "Front brake pads replaced", MileageAt = 25000,
                    Cost = 150.00m, WorkshopName = "Brake Masters"
                },
                new ServiceRecord {
                    VehicleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ServiceDate = new DateTime(2020, 11, 20), Title = "Tire Rotation",
                    Description = "Rotated all four tires", MileageAt = 45000,
                    Cost = 40.00m, WorkshopName = "Tire Pros"
                },
                new ServiceRecord {
                    VehicleId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ServiceDate = new DateTime(2021, 9, 30), Title = "Battery Check",
                    Description = "Battery health inspection and replacement", MileageAt = 18000,
                    Cost = 200.00m, WorkshopName = "Electric Auto"
                },
                new ServiceRecord {
                    VehicleId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ServiceDate = new DateTime(2023, 2, 15), Title = "Transmission Service",
                    Description = "Fluid change and inspection", MileageAt = 60000,
                    Cost = 300.00m, WorkshopName = "GearWorks"
                }
            };

            context.ServiceRecords.AddRange(records);
            await context.SaveChangesAsync();
        }
    }
}
