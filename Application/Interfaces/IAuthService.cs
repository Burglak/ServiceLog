using ServiceLog.Application.DTOs.Auth;

namespace ServiceLog.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task SendResetCodeAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string code, string newPassword);

    }
}
