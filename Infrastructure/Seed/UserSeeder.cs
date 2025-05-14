using Microsoft.AspNetCore.Identity;
using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Seed
{
    [SeederOrder(1)]
    public class UserSeeder : ISeeder
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserSeeder(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Users.Any()) return;

            var users = new[]
            {
                new User { Username = "alice", Email = "alice@example.com", Role = UserRole.User },
                new User { Username = "bob", Email = "bob@example.com", Role = UserRole.User },
                new User { Username = "carol", Email = "carol@example.com", Role = UserRole.User },
                new User { Username = "dave", Email = "dave@example.com", Role = UserRole.User },
                new User { Username = "eve", Email = "eve@example.com", Role = UserRole.User }
            };

            foreach (var user in users)
            {
                user.Password = _passwordHasher.HashPassword(user, "password");
            }

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}
