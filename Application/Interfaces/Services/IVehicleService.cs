using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleRequest request);
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetUserVehiclesAsync();

        Task<Vehicle> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request);
        Task<bool> TransferVehicleOwnershipAsync(Guid vehicleId, int newUserId);
        Task DeleteVehicleAsync(Guid id);
    }
}
