using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface IVehicleUserRepository
    {
        Task<VehicleUser?> GetByIdAsync(int userId, Guid vehicleId);
        Task<IEnumerable<VehicleUser>> GetAllAsync();
        Task<VehicleUser> AddAsync(VehicleUser relation);
        Task DeleteAsync(int userId, Guid vehicleId);
    }
}
