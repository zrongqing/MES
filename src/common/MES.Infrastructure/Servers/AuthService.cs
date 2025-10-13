using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MES.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MES.Infrastructure.Servers;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<UserProfile> _hasher = new();
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<bool> RegisterAsync(string username, string password, string? email = null)
    {
        if (await _db.UserProfiles.AnyAsync(u => u.UserName == username))
            return false;

        var profile = new UserProfile
        {
            UserName = username,
            Email = email
        };

        var credential = new UserCredential
        {
            UserProfile = profile,
            PasswordHash = _hasher.HashPassword(profile, password)
        };

        _db.Add(profile);
        _db.Add(credential);

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">用户密码</param>
    /// <returns></returns>
    public async Task<string?> LoginAsync(string username, string password)
    {
        var profile = await _db.UserProfiles
                          .Include(u => u.Credential)
                          .FirstOrDefaultAsync(u => u.UserName == username);

        if (profile?.Credential == null)
            return null;

        var result = _hasher.VerifyHashedPassword(profile, profile.Credential.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        profile.Credential.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // JWT
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, profile.Id.ToString()),
            new Claim(ClaimTypes.Name, profile.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
