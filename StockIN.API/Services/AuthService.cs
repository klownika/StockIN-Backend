using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockIN.API.Data;
using StockIN.API.DTOs;
using StockIN.API.Models;

namespace StockIN.API.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<UserAccount> _passwordHasher;

    public AuthService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<UserAccount>();
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.UserAccounts
            .Include(u => u.Role)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !user.IsActive)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Username = user.Username,
            Role = user.Role.RoleName,
            Token = string.Empty
        };
    }
}