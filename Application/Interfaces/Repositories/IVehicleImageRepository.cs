using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface IVehicleImageRepository
    {
        Task<VehicleImage?> GetByIdAsync(int id);
        Task<IEnumerable<VehicleImage>> GetAllAsync();
        Task<VehicleImage> AddAsync(VehicleImage image);
        Task<VehicleImage> UpdateAsync(VehicleImage image);
        Task DeleteAsync(int id);
    }
}
