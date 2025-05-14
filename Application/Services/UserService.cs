using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.User;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task<bool> UpdateCurrentUserAsync(UpdateUserRequest dto)
        {
            var userId = GetUserId();
            if (userId == null)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            user.Email = dto.Email;
            user.Username = dto.Username;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCurrentUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private int? GetUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : null;
        }
    }
}
