using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<Notification> AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsAsync(Guid vehicleId);
        Task<Notification?> GetNotificationAsync(int id);
        Task<IEnumerable<Notification>> GetAllForUserAsync();
        Task<bool> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int id);
    }
}
