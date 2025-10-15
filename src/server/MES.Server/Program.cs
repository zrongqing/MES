using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MES.Server.Jwt;

namespace MES.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Json Web Token
        // var jwtConfig = new JwtConfig();
        // builder.Configuration.Bind("JwtConfig", jwtConfig);
        JwtConfig? jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddAuthentication(options =>
            {
                // 默认认证方式
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // 默认认证失败返回方式
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,                                                                      //是否验证Issuer
                    ValidIssuer = jwtConfig?.Issuer,                                                            //发行人Issuer
                    ValidateAudience = true,                                                                    //是否验证Audience
                    ValidAudience = jwtConfig?.Audience,                                                        //订阅人Audience
                    ValidateIssuerSigningKey = true,                                                            //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SecretKey!)), //SecurityKey
                    ValidateLifetime = true,                                                                    //是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(60),                                                       //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            })
            .AddNegotiate();     // 内网 Windows 验证
            

        builder.Services.AddAuthorization();
        // builder.Services.AddAuthorization(options =>
        // {
        //     // By default, all incoming requests will be authorized according to the default policy.
        //     options.FallbackPolicy = options.DefaultPolicy;
        // });
        builder.Services.AddSingleton(new JwtHelper(builder.Configuration));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //调用中间件：UseAuthentication（认证），必须在所有需要身份认证的中间件前调用，比如 UseAuthorization（授权）。
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
