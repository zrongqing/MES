using System.Text;
using App.Server.Data;
using App.Server.Models;
using App.Server.Servers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace App.Server.Security;

public static class AuthenticationExtension
{
    /// <summary>
    /// 添加认证
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>()!;

        // 添加微软的官方认证
        services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();
        services.AddScoped<ITokenService, TokenService>();

        if (jwtConfig.Enabled)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig?.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig?.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SecretKey!)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(60),
                        RequireExpirationTime = true,
                    };
                })
                .AddNegotiate(); // 内网 Windows 验证
        }
        else
        {
            services.AddAuthentication("AllowAll")
                .AddScheme<AuthenticationSchemeOptions, AllowAllHandler>("AllowAll", null);
        }

        return services;
    }
}