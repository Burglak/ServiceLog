using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface IServiceRecordRepository
    {
        Task<ServiceRecord?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceRecord>> GetAllAsync();
        Task<ServiceRecord> AddAsync(ServiceRecord record);
        Task<ServiceRecord> UpdateAsync(ServiceRecord record);
        Task DeleteAsync(int id);
    }
}
