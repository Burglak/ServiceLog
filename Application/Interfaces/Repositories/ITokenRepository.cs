using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;

namespace ServiceLog.Infrastructure.Repositories
{
    public interface ITokenRepository
    {
        Task<Token?> GetValidTokenAsync(string email, string code, TokenType type);
        Task<Token> CreateTokenAsync(string email, TokenType type, string code, DateTime expireAt);
        Task InvalidateTokenAsync(Token token);
    }
}
