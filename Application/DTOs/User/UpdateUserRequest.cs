namespace ServiceLog.Application.DTOs.User
{
    public class UpdateUserRequest
    {
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
