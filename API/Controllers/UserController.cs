using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.Interfaces.Services;
using ServiceLog.Application.DTOs;
using ServiceLog.Application.DTOs.User;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequest dto)
        {
            var result = await _userService.UpdateCurrentUserAsync(dto);
            return result ? Ok("Updated successfully") : BadRequest("Failed to update");
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            var result = await _userService.DeleteCurrentUserAsync();
            return result ? Ok("Account deleted") : BadRequest("Failed to delete");
        }
    }
}
