# 开发记录

## 服务器搭建

### EFCore工具的使用

建立好基本的数据结构，想使用EFCoreTool去直接创建数据库

先是执行 Add Migration 

```csharp
public partial class MesDbContext : DbContext
{
    public DbSet<UserCredential> Users => Set<UserCredential>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    // public MesDbContext(DbContextOptions<MesDbContext> options)
    //     : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=MesDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 如果有 Fluent API 配置，可以这里做
    }
}
```

| 场景                | 连接字符串                                                                                                    |
| ----------------- | -------------------------------------------------------------------------------------------------------- |
| 使用 Windows 身份验证   | `"Server=DESKTOP-12345\\SQLEXPRESS;Database=MesDb;Trusted_Connection=True;TrustServerCertificate=True;"` |
| 使用 SQL 登录账号       | `"Server=localhost\\SQLEXPRESS;Database=MesDb;User Id=sa;Password=123456;TrustServerCertificate=True;"`  |
| 使用默认实例（非 Express） | `"Server=localhost;Database=MesDb;Trusted_Connection=True;TrustServerCertificate=True;"`                 |

教程中默认写的 LocalDb 本地数据库的连接方式不要搞错了

### JWT的使用

[json web token](https://www.cnblogs.com/clis/p/16151872.html)

---

添加JWT认证  

```csharp
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
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
```

DefaultAuthenticateScheme：指定默认的 身份验证方案，也就是 [Authorize] 或 User.Identity 默认使用哪种认证方式。  
DefaultChallengeScheme: 指定默认的 挑战（Challenge）方案，也就是当用户未认证时，系统用哪个方式返回“未授权”响应。  

- JWT：返回 401 Unauthorized
- Cookie：返回登录页面重定向
- Negotiate：触发 Windows 身份验证（NTLM/Kerberos）

---

注意事项：  
- 默认创建的项目是自带了一种认证方式，如果发现认证一直失败，看一下 DefaultAuthenticateScheme 是否配置错了。