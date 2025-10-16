using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Core.Security;
using Microsoft.IdentityModel.Tokens;

namespace App.Server.Security;

public class JwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly JwtConfig _jwtConfig;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
    }

    /// <summary>
    /// 目前这里是测试
    /// </summary>
    /// <returns></returns>
    public string CreateToken()
    {
        // 1. 定义需要使用到的Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "u_admin"), //HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, "r_admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(JwtRegisteredClaimNames.Jti, "admin"),
            new Claim("Username", "Admin"),
            new Claim("Name", "超级管理员"),
            new Claim("Permission", Permissions.UserCreate),
            new Claim("Permission", Permissions.UserUpdate)
        };

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,             //Issuer
            audience: _jwtConfig.Audience,         //Audience
            claims: claims,                        //Claims,
            notBefore: DateTime.Now,               //notBefore
            expires: DateTime.Now.AddMinutes(30),  //expires
            signingCredentials: signingCredentials //Credentials
        );

        // 6. 将token变为string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}