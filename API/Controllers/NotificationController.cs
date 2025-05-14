using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Domain.Entities;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification notification)
        {
            var result = await _notificationService.AddNotificationAsync(notification);
            return Ok(result);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(Guid vehicleId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(vehicleId);
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification == null)
                return NotFound();

            return Ok(notification);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllForUser()
        {
            var result = await _notificationService.GetAllForUserAsync();
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Notification updated)
        {
            if (id != updated.Id)
                return BadRequest("ID mismatch");

            var result = await _notificationService.UpdateNotificationAsync(updated);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _notificationService.DeleteNotificationAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
