using ServiceLog.Application.DTOs.User;

namespace ServiceLog.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetCurrentUserAsync();
        Task<bool> UpdateCurrentUserAsync(UpdateUserRequest dto);
        Task<bool> DeleteCurrentUserAsync();
    }
}
