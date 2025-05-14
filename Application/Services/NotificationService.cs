using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == notification.VehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(Guid vehicleId)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == vehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            return await _context.Notifications
                .Where(n => n.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<Notification?> GetNotificationAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notification == null)
                return null;

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == notification.VehicleId && vu.UserId == userId);

            return isOwner ? notification : null;
        }

        public async Task<IEnumerable<Notification>> GetAllForUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var vehicleIds = await _context.VehicleUsers
                .Where(vu => vu.UserId == userId)
                .Select(vu => vu.VehicleId)
                .ToListAsync();

            return await _context.Notifications
                .Where(n => vehicleIds.Contains(n.VehicleId))
                .ToListAsync();
        }


        public async Task<bool> UpdateNotificationAsync(Notification updated)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var existing = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == updated.Id);

            if (existing == null)
                return false;

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == existing.VehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.NotifyAt = updated.NotifyAt;

            _context.Notifications.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notification == null)
                return false;

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == notification.VehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        private int? GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
