using MES.Infrastructure.Servers;
using Microsoft.AspNetCore.Mvc;

namespace MES.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest req)
    {
        var success = await _authService.RegisterAsync(req.UserName, req.Password);
        return success ? Ok("注册成功") : BadRequest("用户名已存在");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var token = await _authService.LoginAsync(req.UserName, req.Password);
        return token == null ? Unauthorized("用户名或密码错误") : Ok(new { token });
    }
}

public record LoginRequest(string UserName, string Password);