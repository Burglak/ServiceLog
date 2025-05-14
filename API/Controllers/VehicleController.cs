using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.DTOs.Vehicle;
using ServiceLog.Application.Interfaces.Services;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    [Authorize(Roles = "User")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            var result = await _vehicleService.CreateVehicleAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound("Vehicle not found or does not belong to the user");

            return Ok(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForUser()
        {
            try
            {
                var vehicles = await _vehicleService.GetUserVehiclesAsync();
                return Ok(vehicles);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User not logged in");
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            try
            {
                var updatedVehicle = await _vehicleService.UpdateVehicleAsync(id, request);
                return Ok(updatedVehicle);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User not logged in");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{vehicleId}/transfer")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TransferOwnership(Guid vehicleId, [FromBody] TransferVehicleRequest request)
        {
            var success = await _vehicleService.TransferVehicleOwnershipAsync(vehicleId, request.NewUserId);
            if (!success)
                return BadRequest("Transfer failed");

            return Ok("Vehicle ownership transferred successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User not logged in");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
