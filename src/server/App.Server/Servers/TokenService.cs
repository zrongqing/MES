using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using App.Server.Models;
using App.Server.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace App.Server.Servers;

public interface ITokenService
{
    /// <summary>
    /// 创建Token
    /// </summary>
    string CreateToken(ApplicationUser user, IList<string> roles);

    /// <summary>
    /// 生成并存储刷新 token
    /// </summary>
    public Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(ApplicationUser user);

    /// <summary>
    /// 读取并验证刷新 token
    /// </summary>
    public Task<bool> ValidateRefreshTokenAsync(ApplicationUser user, string token);

    /// <summary>
    /// 删除 token（用户登出或主动清除）
    /// </summary>
    public Task RemoveRefreshTokenAsync(ApplicationUser user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IConfiguration config,UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _userManager =  userManager;
    }
      
    public async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(ApplicationUser user)
    {
        // 将 token + 过期时间序列化存储
        var refreshToken = RefreshToken.Generate();
        var value = JsonSerializer.Serialize(refreshToken);
        await _userManager.SetAuthenticationTokenAsync(user, "AppClient", "RefreshToken", value);
        return refreshToken;
    }
   
    public async Task<bool> ValidateRefreshTokenAsync(ApplicationUser user, string token)
    {
        var value = await _userManager.GetAuthenticationTokenAsync(user, "AppClient", "RefreshToken");
        if (string.IsNullOrEmpty(value)) return false;

        var storedToken = JsonSerializer.Deserialize<RefreshToken>(value);
        if (storedToken == null) return false;

        // 校验 token 是否匹配且未过期
        return storedToken.Token == token && !storedToken.IsExpired;
    }
  
    public async Task RemoveRefreshTokenAsync(ApplicationUser user)
    {
        await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
    }
        
    public string CreateToken(ApplicationUser user, IList<string> roles)
    {
        var jwtConfig = _config.GetSection("JwtConfig").Get<JwtConfig>()!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? "")
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience:jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfig.Expired),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}