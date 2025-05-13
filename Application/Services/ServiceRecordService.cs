using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.ServiceRecord;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
{
    public class ServiceRecordService : IServiceRecordService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceRecordService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int? GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        private async Task<bool> OwnsVehicleAsync(Guid vehicleId, int userId)
        {
            return await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == vehicleId && vu.UserId == userId);
        }

        public async Task<ServiceRecord> CreateAsync(CreateServiceRecordRequest request)
        {
            var userId = GetUserId() ?? throw new UnauthorizedAccessException();
            if (!await OwnsVehicleAsync(request.VehicleId, userId))
                throw new UnauthorizedAccessException();

            var record = new ServiceRecord
            {
                VehicleId = request.VehicleId,
                ServiceDate = request.ServiceDate,
                Title = request.Title,
                Description = request.Description,
                MileageAt = request.MileageAt,
                Cost = request.Cost,
                WorkshopName = request.WorkshopName
            };

            _context.ServiceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<ServiceRecord?> GetByIdAsync(int id)
        {
            var userId = GetUserId() ?? throw new UnauthorizedAccessException();
            var record = await _context.ServiceRecords.FindAsync(id);
            if (record == null) return null;

            if (!await OwnsVehicleAsync(record.VehicleId, userId))
                return null;

            return record;
        }

        public async Task<IEnumerable<ServiceRecord>> GetAllAsync()
        {
            var userId = GetUserId() ?? throw new UnauthorizedAccessException();
            var vehicleIds = await _context.VehicleUsers
                .Where(vu => vu.UserId == userId)
                .Select(vu => vu.VehicleId)
                .ToListAsync();

            return await _context.ServiceRecords
                .Where(sr => vehicleIds.Contains(sr.VehicleId))
                .ToListAsync();
        }

        public async Task<ServiceRecord> UpdateAsync(int id, UpdateServiceRecordRequest request)
        {
            var userId = GetUserId() ?? throw new UnauthorizedAccessException();
            var record = await _context.ServiceRecords.FindAsync(id)
                ?? throw new InvalidOperationException("Record not found");

            if (!await OwnsVehicleAsync(record.VehicleId, userId))
                throw new UnauthorizedAccessException();

            if (request.ServiceDate.HasValue) record.ServiceDate = request.ServiceDate.Value;
            if (request.Title != null) record.Title = request.Title;
            if (request.Description != null) record.Description = request.Description;
            if (request.MileageAt.HasValue) record.MileageAt = request.MileageAt.Value;
            if (request.Cost.HasValue) record.Cost = request.Cost.Value;
            if (request.WorkshopName != null) record.WorkshopName = request.WorkshopName;

            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetUserId() ?? throw new UnauthorizedAccessException();
            var record = await _context.ServiceRecords.FindAsync(id)
                ?? throw new InvalidOperationException("Record not found");

            if (!await OwnsVehicleAsync(record.VehicleId, userId))
                throw new UnauthorizedAccessException();

            _context.ServiceRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}
