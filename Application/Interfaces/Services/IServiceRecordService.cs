using ServiceLog.Application.DTOs.ServiceRecord;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IServiceRecordService
    {
        Task<ServiceRecord> CreateAsync(CreateServiceRecordRequest request);
        Task<ServiceRecord?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceRecord>> GetAllAsync();
        Task<ServiceRecord> UpdateAsync(int id, UpdateServiceRecordRequest request);
        Task DeleteAsync(int id);
    }
}
