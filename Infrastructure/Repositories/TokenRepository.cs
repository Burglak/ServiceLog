using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Token> CreateTokenAsync(string email, TokenType type, string code, DateTime expireAt)
        {
            var token = new Token
            {
                Email = email,
                Code = code,
                TokenType = type,
                ExpireAt = expireAt
            };

            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<Token?> GetValidTokenAsync(string email, string code, TokenType type)
        {
            return await _context.Tokens
                .Where(t => t.Email == email && t.Code == code && t.TokenType == type && t.ExpireAt > DateTime.UtcNow)
                .OrderByDescending(t => t.ExpireAt)
                .FirstOrDefaultAsync();
        }

        public async Task InvalidateTokenAsync(Token token)
        {
            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
