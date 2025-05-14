using Microsoft.Extensions.DependencyInjection;
using ServiceLog.Infrastructure.Data;
using System.Reflection;

namespace ServiceLog.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db, IServiceProvider serviceProvider)
        {
            var seederTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ISeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Order = t.GetCustomAttribute<SeederOrderAttribute>()?.Order ?? 0
                })
                .OrderBy(x => x.Order)
                .Select(x => x.Type);

            foreach (var type in seederTypes)
            {
                var seeder = (ISeeder)serviceProvider.GetRequiredService(type);
                await seeder.SeedAsync(db);
            }
        }
    }

}
