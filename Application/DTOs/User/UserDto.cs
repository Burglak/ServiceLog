namespace ServiceLog.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
