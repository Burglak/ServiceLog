using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle> UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Guid id);
        Task<Vehicle?> GetWithDetailsAsync(Guid id);
    }
}
