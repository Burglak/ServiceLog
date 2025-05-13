namespace ServiceLog.API.Controllers
{
    using global::ServiceLog.Application.DTOs.Vehicle;
    using global::ServiceLog.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace ServiceLog.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class VehicleController : ControllerBase
        {
            private readonly IVehicleService _vehicleService;

            public VehicleController(IVehicleService vehicleService)
            {
                _vehicleService = vehicleService;
            }

            [HttpPost]
            [Authorize(Roles = "User")]
            public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
            {
                var result = await _vehicleService.CreateVehicleAsync(request);
                return Ok(result);
            }
        }
    }

}
