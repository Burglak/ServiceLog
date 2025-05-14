using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces;
using ServiceLog.Application.Interfaces.Services;
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
        var expiresInMinutes = 15;

        await _tokenRepository.CreateTokenAsync(
            email,
            TokenType.ResetPassword,
            code,
            DateTime.UtcNow.AddMinutes(expiresInMinutes)
        );

        var textBody = $@"
Hello {user.Username},

Your password reset code is: {code}

This code will expire in {expiresInMinutes} minutes.

If you did not request a password reset, please ignore this email.";

        // HTML body
        var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <title>Password Reset</title>
</head>
<body style=""font-family:Arial,sans-serif; background-color:#f4f4f7; margin:0; padding:20px;"">
  <table width=""100%"" style=""max-width:600px; margin:0 auto; background:white; border-radius:8px; overflow:hidden;"">
    <tr style=""background:#3869D4; color:white;"">
      <td style=""padding:20px; text-align:center; font-size:24px;"">
        ServiceLog Password Reset
      </td>
    </tr>
    <tr>
      <td style=""padding:20px; color:#51545E; font-size:16px;"">
        <p>Hello <strong>{user.Username}</strong>,</p>
        <p>You recently requested to reset your password. Use the code below to proceed:</p>
        <p style=""text-align:center; margin:30px 0;"">
          <span style=""display:inline-block; padding:15px 25px; font-size:20px; letter-spacing:2px; background:#F1F1F1; border-radius:4px;"">
            {code}
          </span>
        </p>
        <p>This code will <strong>expire in {expiresInMinutes} minutes</strong>.</p>
        <p>If you did not request a password reset, please ignore this email. No changes will be made to your account.</p>
        <p>Thanks,<br/>The ServiceLog Team</p>
      </td>
    </tr>
    <tr style=""background:#F4F4F7; text-align:center; font-size:12px; color:#A8AAAF; padding:10px;"">
      <td style=""padding:20px;"">
        © {DateTime.UtcNow.Year} ServiceLog. All rights reserved.
      </td>
    </tr>
  </table>
</body>
</html>";

        await _emailService.SendEmailAsync(email,
            "Your ServiceLog Password Reset Code",
            textBody,
            htmlBody);
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
