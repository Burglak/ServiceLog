using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace ServiceLog.Infrastructure.Seed
{
    [SeederOrder(5)]
    public class NotificationSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Notifications.Any()) return;

            var notes = new[]
            {
                new Notification {
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Oil Change Due", Description = "Time for next oil change at 35k miles",
                    NotifyAt = DateTime.UtcNow.AddDays(7)
                },
                new Notification {
                    VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "Brake Inspection", Description = "Inspect brakes before winter",
                    NotifyAt = DateTime.UtcNow.AddDays(30)
                },
                new Notification {
                    VehicleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Title = "Tire Rotation Reminder", Description = "Rotate tires every 6 months",
                    NotifyAt = DateTime.UtcNow.AddMonths(6)
                },
                new Notification {
                    VehicleId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Title = "Battery Health Check", Description = "Check battery before long trip",
                    NotifyAt = DateTime.UtcNow.AddDays(14)
                },
                new Notification {
                    VehicleId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Title = "Transmission Service Alert", Description = "Schedule transmission service",
                    NotifyAt = DateTime.UtcNow.AddDays(90)
                }
            };

            context.Notifications.AddRange(notes);
            await context.SaveChangesAsync();
        }
    }
}
