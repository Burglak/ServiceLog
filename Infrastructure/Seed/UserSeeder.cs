using Microsoft.AspNetCore.Identity;
using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Seed
{
    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Users.Any())
                return;

            var hasher = new PasswordHasher<User>();

            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Role = UserRole.Admin
                },
                new User
                {
                    Username = "user1",
                    Email = "user1@example.com",
                    Role = UserRole.User
                }
            };

            foreach(var user in users)
            {
                user.Password = hasher.HashPassword(user, "password");
            }

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }
    }
}
