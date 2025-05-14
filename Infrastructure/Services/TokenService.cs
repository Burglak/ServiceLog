using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;
using ServiceLog.Infrastructure.Repositories;

public class TokenService : ITokenService
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public TokenService(
        ITokenRepository tokenRepository,
        IEmailService emailService,
        ApplicationDbContext context)
    {
        _tokenRepository = tokenRepository;
        _emailService = emailService;
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new Exception("User with this email does not exist.");

        var code = GenerateCode();

        await _tokenRepository.CreateTokenAsync(
            email,
            TokenType.ResetPassword,
            code,
            DateTime.UtcNow.AddMinutes(15)
        );

        await _emailService.SendEmailAsync(email, "Reset your password", $"Your reset code is: {code}");
    }

    public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
    {
        var token = await _tokenRepository.GetValidTokenAsync(email, code, TokenType.ResetPassword);
        if (token == null)
            return false;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return false;

        user.Password = _passwordHasher.HashPassword(user, newPassword);
        _context.Users.Update(user);

        await _tokenRepository.InvalidateTokenAsync(token);

        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateCode()
    {
        var random = new Random();
        return random.Next(100_000, 999_999).ToString();
    }
}
