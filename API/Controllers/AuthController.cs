using Microsoft.AspNetCore.Mvc;
using ServiceLog.Application.DTOs.Auth;
using ServiceLog.Application.Interfaces;

namespace ServiceLog.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var token = await _authService.RegisterAsync(request);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            return Ok(new { token });
        }

        [HttpPost("send-reset-code")]
        public async Task<IActionResult> SendResetCode(SendResetCodeRequest request)
        {
            await _authService.SendResetCodeAsync(request.Email);
            return Ok(new { message = "Reset code sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var success = await _authService.ResetPasswordAsync(request.Email, request.Code, request.NewPassword);
            if (!success)
                return BadRequest(new { message = "Invalid code or email." });

            return Ok(new { message = "Password has been reset successfully." });
        }

    }
}
