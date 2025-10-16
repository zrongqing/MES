# Authentication

登录认证使用的是微软集成的一套。  
[ASP.NET Authentication](https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/?view=aspnetcore-8.0)  

![alt text](images/ChatGPT%20Image%20Oct%2021,%202025,%2008_35_21%20PM.png)

## 基本数据结构

```csharp
public class ApplicationUser : IdentityUser
{
    // 扩展字段（如需要）
    public string FullName { get; set; }
}
```

```csharp
public class ApplicationRole: IdentityRole
{
    
}
```

## Token生成服务

```csharp
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
```

## 认证接口

```csharp
[ApiController]
[Route("[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(
        UserManager<ApplicationUser>   userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService                  tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User created successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid username or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.CreateToken(user, roles);
        var refreshToken = await _tokenService.GenerateAndStoreRefreshTokenAsync(user);
            
        return Ok(new   
        {
            Token = token,
            RefreshToken = refreshToken.Token
        });

        return Ok(new { token });
    }
        
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var valid = await _tokenService.ValidateRefreshTokenAsync(user, dto.RefreshToken);

        if (!valid)
            return Unauthorized("Invalid refresh token.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.CreateToken(user, roles);
        var newRefreshToken = await _tokenService.GenerateAndStoreRefreshTokenAsync(user);
            
        return Ok(new   
        {
            Token = token,
            RefreshToken = newRefreshToken.Token,
        });

        return Ok(new { token });
    }
        

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password.");
            
        return Unauthorized("Invalid username or password.");
    }
 
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Me()
    {
        return Ok(new
        {
            User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
```

## IOC接入

```CSharp
// 添加微软的官方认证
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
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
builder.Services.AddScoped<ITokenService, TokenService>();
```

## EFCore接入

EFCore可以直接使用框架封装好的，像这样

```csharp
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
```

或者可以参考官方文档[ASP.NET Core 中的 Identity 模型自定义](https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0)