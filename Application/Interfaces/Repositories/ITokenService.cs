namespace ServiceLog.Application.Interfaces.Repositories
{
    public interface ITokenService
    {
        Task GenerateResetPasswordTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string code, string newPassword);
    }
}
