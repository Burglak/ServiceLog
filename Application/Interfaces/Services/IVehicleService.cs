using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleRequest request);
    }
}
