using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.DTOs.Auth;
using ServiceLog.Application.Interfaces;
using ServiceLog.Application.Interfaces.Repositories;
using ServiceLog.Domain.Entities;
using ServiceLog.Domain.Enums;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService, ITokenService tokenService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = UserRole.User
            };

            user.Password = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(user.Id, user.Username, user.Role.ToString());
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password!, request.Password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Invalid credentials");

            return _jwtService.GenerateToken(user.Id, user.Username, user.Role.ToString());
        }

        public async Task SendResetCodeAsync(string email)
        {
            await _tokenService.GenerateResetPasswordTokenAsync(email);
        }

        public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
        {
            return await _tokenService.ResetPasswordAsync(email, code, newPassword);
        }
    }
}
