// using App.Core.Security;
// using MES.Server.Servers;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity.Data;
// using Microsoft.AspNetCore.Mvc;
//
// namespace MES.Server.Controllers;
//
// [Route("[controller]/[action]")]
// [ApiController]
// public class UserController : ControllerBase
// {
//     [HttpGet]
//     [Authorize(Permissions.UserCreate)]
//     public ActionResult<string> T_UserCreate() => "UserCreate";
//
//     [HttpGet]
//     [Authorize(Permissions.UserUpdate)]
//     public ActionResult<string> T_UserUpdate() => "UserUpdate";
//
//     [HttpGet]
//     [Authorize(Permissions.UserDelete)]
//     public ActionResult<string> T_UserDelete() => "UserDelete";
//
//     private readonly ILogger<UserController> _logger;
//     public UserController(ILogger<UserController> logger)
//     {
//         _logger = logger;
//     }
//     
//     [HttpGet]
//     public async Task<IActionResult> GetUsers()
//     {
//         return Ok("GetUsers");
//     }
//     
//     [HttpPost]
//     public async Task<IActionResult> Login([FromBody] LoginRequest request)
//     {
//         string token = "GetToken";
//         return Ok(token);
//     }
//
//     public async Task<IActionResult> Register([FromBody] RegisterRequest request)
//     {
//         string token = "GetToken";
//         return Ok(token);
//     }
//     
// }
