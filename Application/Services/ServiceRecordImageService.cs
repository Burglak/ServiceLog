using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
{
    public class ServiceRecordImageService : IServiceRecordImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _imageRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "service-record-images");

        public ServiceRecordImageService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

            if (!Directory.Exists(_imageRootPath))
                Directory.CreateDirectory(_imageRootPath);
        }

        public async Task<ServiceRecordImage> AddImageAsync(int serviceRecordId, IFormFile file)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var serviceRecord = await _context.ServiceRecords
                .FirstOrDefaultAsync(sr => sr.Id == serviceRecordId);

            if (serviceRecord == null)
                throw new UnauthorizedAccessException("Service record not found");

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == serviceRecord.VehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Access denied");

            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(_imageRootPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new ServiceRecordImage
            {
                ServiceRecordId = serviceRecordId,
                ImagePath = Path.Combine("uploads", "service-record-images", fileName).Replace("\\", "/")
            };

            _context.ServiceRecordImages.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<IEnumerable<ServiceRecordImage>> GetAllForUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var vehicleIds = await _context.VehicleUsers
                .Where(vu => vu.UserId == userId)
                .Select(vu => vu.VehicleId)
                .ToListAsync();

            return await _context.ServiceRecordImages
                .Include(i => i.ServiceRecord)
                .Where(i => vehicleIds.Contains(i.ServiceRecord.VehicleId))
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecordImage>> GetAllForServiceRecordAsync(int serviceRecordId)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException();

            var record = await _context.ServiceRecords
                .Include(sr => sr.Vehicle)
                .FirstOrDefaultAsync(sr => sr.Id == serviceRecordId);

            if (record == null)
                return [];

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.UserId == userId && vu.VehicleId == record.VehicleId);

            if (!isOwner)
                throw new UnauthorizedAccessException();

            return await _context.ServiceRecordImages
                .Where(i => i.ServiceRecordId == serviceRecordId)
                .ToListAsync();
        }
        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var image = await _context.ServiceRecordImages
                .Include(i => i.ServiceRecord)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
                return false;

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == image.ServiceRecord.VehicleId && vu.UserId == userId);

            if (!isOwner)
                throw new UnauthorizedAccessException("Access denied");

            var fullPath = Path.Combine("wwwroot", image.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.ServiceRecordImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<FileStreamResult?> GetImageAsync(int imageId)
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            var image = await _context.ServiceRecordImages
                .Include(i => i.ServiceRecord)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
                return null;

            var isOwner = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == image.ServiceRecord.VehicleId && vu.UserId == userId);

            if (!isOwner)
                return null;

            var fullPath = Path.Combine("wwwroot", image.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!File.Exists(fullPath))
                return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var contentType = "image/jpeg";

            return new FileStreamResult(stream, contentType);
        }

        private int? GetUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : null;
        }
    }
}
