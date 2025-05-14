namespace ServiceLog.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task GenerateResetPasswordTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string code, string newPassword);
    }
}
