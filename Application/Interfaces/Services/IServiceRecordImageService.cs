﻿using Microsoft.AspNetCore.Mvc;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IServiceRecordImageService
    {
        Task<ServiceRecordImage> AddImageAsync(int serviceRecordId, IFormFile file);
        Task<bool> DeleteImageAsync(int imageId);
        Task<IEnumerable<ServiceRecordImage>> GetAllForUserAsync();
        Task<IEnumerable<ServiceRecordImage>> GetAllForServiceRecordAsync(int serviceRecordId);
        Task<FileStreamResult?> GetImageAsync(int imageId);
    }
}
