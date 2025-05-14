using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Application.Services;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/vehicle-images")]
    [Authorize(Roles = "User")]
    public class VehicleImageController : ControllerBase
    {
        private readonly IVehicleImageService _imageService;

        public VehicleImageController(IVehicleImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> Upload(Guid vehicleId, IFormFile file)
        {
            try
            {
                var result = await _imageService.AddImageAsync(vehicleId, file);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForUser()
        {
            var result = await _imageService.GetAllForUserAsync();
            return Ok(result);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetAllForVehicle(Guid vehicleId)
        {
            var result = await _imageService.GetAllForVehicleAsync(vehicleId);
            return Ok(result);
        }


        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var imageStream = await _imageService.GetImageAsync(imageId);

            if (imageStream == null)
            {
                return NotFound("Image not found or you don't have access to this vehicle");
            }

            return imageStream;
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            try
            {
                var success = await _imageService.DeleteImageAsync(imageId);
                if (!success)
                    return NotFound("Image not found");

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
