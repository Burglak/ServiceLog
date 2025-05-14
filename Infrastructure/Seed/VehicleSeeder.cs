using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Seed
{
    [SeederOrder(2)]
    public class VehicleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Vehicles.Any()) return;

            var vehicles = new[]
            {
                new Vehicle {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Make = "Toyota", Model = "Corolla", Year = 2018,
                    VehicleType = VehicleType.Car, Vin = "JTDBU4EE9B9123456",
                    RegistrationNumber = "ABC1234", EngineType = EngineType.Petrol,
                    EngineCapacity = 1.8m, Power = 132, Mileage = 50000,
                    FirstRegistration = new DateTime(2018, 5, 20), Color = VehicleColor.White
                },
                new Vehicle {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Make = "Honda", Model = "Civic", Year = 2020,
                    VehicleType = VehicleType.Car, Vin = "2HGFC2F69LH123456",
                    RegistrationNumber = "DEF5678", EngineType = EngineType.Petrol,
                    EngineCapacity = 2.0m, Power = 158, Mileage = 30000,
                    FirstRegistration = new DateTime(2020, 3, 10), Color = VehicleColor.Red
                },
                new Vehicle {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Make = "Ford", Model = "F-150", Year = 2019,
                    VehicleType = VehicleType.Truck, Vin = "1FTFW1E50KFB12345",
                    RegistrationNumber = "GHI9012", EngineType = EngineType.Petrol,
                    EngineCapacity = 3.3m, Power = 290, Mileage = 60000,
                    FirstRegistration = new DateTime(2019, 7, 5), Color = VehicleColor.Blue
                },
                new Vehicle {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Make = "Tesla", Model = "Model 3", Year = 2021,
                    VehicleType = VehicleType.Car, Vin = "5YJ3E1EA7MF123456",
                    RegistrationNumber = "JKL3456", EngineType = EngineType.Electric,
                    EngineCapacity = 0m, Power = 258, Mileage = 20000,
                    FirstRegistration = new DateTime(2021, 1, 15), Color = VehicleColor.Red
                },
                new Vehicle {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Make = "BMW", Model = "X5", Year = 2017,
                    VehicleType = VehicleType.Car, Vin = "5UXKR0C50H0X12345",
                    RegistrationNumber = "MNO7890", EngineType = EngineType.Diesel,
                    EngineCapacity = 3.0m, Power = 262, Mileage = 80000,
                    FirstRegistration = new DateTime(2017, 9, 1), Color = VehicleColor.Black
                }
            };

            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();
        }
    }
}
