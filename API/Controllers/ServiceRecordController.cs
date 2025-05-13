using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.DTOs.ServiceRecord;
using ServiceLog.Application.Interfaces.Services;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/service-records")]
    [Authorize(Roles = "User")]
    public class ServiceRecordController : ControllerBase
    {
        private readonly IServiceRecordService _service;

        public ServiceRecordController(IServiceRecordService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRecordRequest dto)
        {
            try
            {
                var rec = await _service.CreateAsync(dto);
                return Ok(rec);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var rec = await _service.GetByIdAsync(id);
            if (rec == null) return NotFound();
            return Ok(rec);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceRecordRequest dto)
        {
            try
            {
                var rec = await _service.UpdateAsync(id, dto);
                return Ok(rec);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
