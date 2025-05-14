using Microsoft.AspNetCore.Mvc;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IVehicleImageService
    {
        Task<VehicleImage> AddImageAsync(Guid vehicleId, IFormFile file);
        Task<FileStreamResult?> GetImageAsync(int imageId);
        Task<IEnumerable<VehicleImage>> GetAllForUserAsync();
        Task<IEnumerable<VehicleImage>> GetAllForVehicleAsync(Guid vehicleId);

        Task<bool> DeleteImageAsync(int imageId);
    }
}
