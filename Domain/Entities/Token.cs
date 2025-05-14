using ServiceLog.Domain.Enums;

namespace ServiceLog.Domain.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public TokenType TokenType { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
