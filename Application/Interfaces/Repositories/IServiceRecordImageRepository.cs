using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface IServiceRecordImageRepository
    {
        Task<ServiceRecordImage?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceRecordImage>> GetAllAsync();
        Task<ServiceRecordImage> AddAsync(ServiceRecordImage image);
        Task<ServiceRecordImage> UpdateAsync(ServiceRecordImage image);
        Task DeleteAsync(int id);
    }
}
