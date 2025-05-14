using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;
using ServiceLog.Infrastructure.Data;
using System.Security.Claims;

namespace ServiceLog.Application.Services
{
    public class VehicleImageService : IVehicleImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _imageRootPath;

        public VehicleImageService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _imageRootPath = Path.Combine(env.WebRootPath, "uploads", "vehicle-images");

            if (!Directory.Exists(_imageRootPath))
                Directory.CreateDirectory(_imageRootPath);
        }

        public async Task<VehicleImage> AddImageAsync(Guid vehicleId, IFormFile file)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var isOwned = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == vehicleId && vu.UserId == userId);

            if (!isOwned)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var savePath = Path.Combine(_imageRootPath, uniqueFileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new VehicleImage
            {
                VehicleId = vehicleId,
                ImagePath = Path.Combine("uploads", "vehicle-images", uniqueFileName).Replace("\\", "/")
            };

            _context.VehicleImages.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<IEnumerable<VehicleImage>> GetAllForUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var vehicleIds = await _context.VehicleUsers
                .Where(vu => vu.UserId == userId)
                .Select(vu => vu.VehicleId)
                .ToListAsync();

            return await _context.VehicleImages
                .Where(i => vehicleIds.Contains(i.VehicleId))
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleImage>> GetAllForVehicleAsync(Guid vehicleId)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var isOwned = await _context.VehicleUsers
                .AnyAsync(vu => vu.UserId == userId && vu.VehicleId == vehicleId);

            if (!isOwned)
                throw new UnauthorizedAccessException("Vehicle does not belong to user");

            return await _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<FileStreamResult?> GetImageAsync(int imageId)
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            var image = await _context.VehicleImages
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
                return null;

            var hasAccess = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == image.VehicleId && vu.UserId == userId);

            if (!hasAccess)
                return null;

            var fullPath = Path.Combine("wwwroot", image.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!File.Exists(fullPath))
                return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var contentType = "image/jpeg";

            return new FileStreamResult(stream, contentType);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var userId = GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var image = await _context.VehicleImages
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
                return false;

            var hasAccess = await _context.VehicleUsers
                .AnyAsync(vu => vu.VehicleId == image.VehicleId && vu.UserId == userId);

            if (!hasAccess)
                return false;

            var fullPath = Path.Combine("wwwroot", image.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.VehicleImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }


        private int? GetUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : null;
        }
    }
}
