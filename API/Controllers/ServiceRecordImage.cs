using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.Interfaces.Services;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/service-record-images")]
    public class ServiceRecordImageController : ControllerBase
    {
        private readonly IServiceRecordImageService _service;

        public ServiceRecordImageController(IServiceRecordImageService service)
        {
            _service = service;
        }

        [HttpPost("{serviceRecordId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadImage(int serviceRecordId, IFormFile file)
        {
            var image = await _service.AddImageAsync(serviceRecordId, file);
            return Ok(image);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForUser()
        {
            var result = await _service.GetAllForUserAsync();
            return Ok(result);
        }

        [HttpGet("record/{serviceRecordId}")]
        public async Task<IActionResult> GetAllForServiceRecord(int serviceRecordId)
        {
            var result = await _service.GetAllForServiceRecordAsync(serviceRecordId);
            return Ok(result);
        }


        [HttpGet("{imageId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var result = await _service.GetImageAsync(imageId);
            if (result == null)
                return NotFound("Image not found or access denied");

            return result;
        }

        [HttpDelete("{imageId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var success = await _service.DeleteImageAsync(imageId);
            if (!success)
                return NotFound("Image not found or access denied");

            return NoContent();
        }
    }
}
