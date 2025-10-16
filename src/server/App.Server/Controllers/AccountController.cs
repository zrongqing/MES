using App.Server.Models;
using App.Server.Servers;
using App.Sharp.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Server.Controllers;

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