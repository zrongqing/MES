# 开发记录

记录开发过程所用的技术，以及遇到的一些疑问点

# EFCore Tool

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

# Json Web Token

参考链接：  
---

[json web token](https://www.cnblogs.com/clis/p/16151872.html)

---

## 添加JWT认证  

WT（Json Web Token）做的事：

- 登录时，服务端生成一个 JWT Token  
- 客户端每次请求都带上这个 token（放在 Header）  
- 服务器通过验证 token 确认身份（是否有效、是否过期、是谁签发的）  

证明“你是谁”

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

## 添加授权

授权基于认证，在启用了认证的情况下，如果没有完成认证（服务器不知道是你是谁），将不会执行授权相关逻辑。  

相关标签：Authorize 和 AllowAnonymous  
授权方式：介绍三种授权方式（Policy、Role、Scheme）  
基于策略（Policy）的授权：深入 Policy 授权方式 

## 添加标签

Authorize：  

打上该标签的 Controller 或 Action 必须经过认证，且可以标识需要满足哪些授权规则。

```csharp
[Authorize(Policy = "", Roles ="", AuthenticationSchemes ="")]
```
Policy（策略）、Roles（角色） 或 AuthenticationSchemes（方案）

---

AllowAnonymou：  

允许匿名访问，级别高于 [Authorize] ，若两者同时作用，将生效 [AllowAnonymous]  

## Policy（策略）授权

推荐授权方式。一个 Policy 可以包含多个要求（要求可能是 Role 匹配，也可能是 Claims 匹配，也可能是其他方式。）

### 定义权限常量（Permissions）

```csharp
public static class Permissions
{
    public const string User = "User";
    public const string UserCreate = $"{User}.Create";
    public const string UserUpdate = $"{User}.Update";
    public const string UserDelete = $"{User}.Delete";
}
```

如上，定义了“增”、“删”、“改”等权限，其中 User 将拥有完整权限。  

### 定义授权需求（Requirement）

```csharp
public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionAuthorizationRequirement(string permission)
    {
        Permission = permission;
    }
}
```

Permission 属性表示权限的名称，与 Permissions 中的常量对应。  

### 编写授权处理器（Handler）

```csharp
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        // 从用户Claims中取出权限列表
        var permissions = context.User
            .FindAll("Permission")
            .Select(c => c.Value).ToList();

        // 如果包含所需权限，就通过验证
        if (permissions.Any(s => s.StartsWith(requirement.Permission)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

### 注册授权策略

AuthorizationExtension.cs
```csharp
services.AddAuthorizationBuilder()
    .AddPolicy(Permissions.UserCreate, policy =>
        policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserCreate)))
    .AddPolicy(Permissions.UserUpdate, policy =>
        policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserUpdate)))
    .AddPolicy(Permissions.UserDelete, policy =>
        policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserDelete)));
```

### 控制器的使用

```csharp
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = Permissions.UserCreate)]
    public IActionResult CreateUser()
    {
        return Ok("用户创建成功");
    }

    [HttpPost]
    [Authorize(Policy = Permissions.UserDelete)]
    public IActionResult DeleteUser()
    {
        return Ok("用户删除成功");
    }
}
```

### 执行流程图

```pgsql
🔹 用户访问接口 (POST /api/User/CreateUser)
       │
       ▼
🔹 JWT 中的 Claims 被解析（认证阶段）
       │
       ▼
🔹 找到 [Authorize(Policy="User.Create")] 声明的策略
       │
       ▼
🔹 ASP.NET Core 调用 PermissionAuthorizationHandler
       │
       ▼
🔹 Handler 检查用户 Claims 中是否包含 "Permission": "User.Create"
       │
       ├─✅ 有 → 放行
       └─❌ 无 → 返回 403 Forbidden
```

### 结合JWT的授权方式

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim("Permission", Permissions.UserCreate),
    new Claim("Permission", Permissions.UserUpdate)
};

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
var token = new JwtSecurityToken(
    issuer: jwtConfig.Issuer,
    audience: jwtConfig.Audience,
    claims: claims,
    expires: DateTime.Now.AddHours(1),
    signingCredentials: creds
);

string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
```

#### Role（角色）

基于角色授权，只要用户拥有角色，即可通过授权验证。

在认证时，给用户添加角色相关的 Claim ，即可标识用户拥有的角色（注：一个用户可以拥有多个角色的 Claim），如：
```csharp
new Claim(ClaimTypes.Role, "admin"),
new Claim(ClaimTypes.Role, "user")
```

在 Controller 或 Action 中:  
```csharp

[Authorize(Roles = "user")]
public class TestController : ControllerBase
{
    public ActionResult<string> GetUser => "GetUser";
    
    [Authorize(Roles = "admin")] //与控制器的Authorize叠加作用，除了拥有user，还需拥有admin
    public ActionResult<string> GetAdmin => "GetAdmin";
    
    [Authorize(Roles = "user,admin")] //user 或 admin 其一满足即可
    public ActionResult<string> GetUserOrAdmin => "GetUserOrAdmin";
}

```

#### Scheme(方案)

方案如：Cookies 和 Bearer，当然也可以是自定义的方案。 
由于这种方式不常用，这里不做展开，请参考官方文档按方案限制标识。  

