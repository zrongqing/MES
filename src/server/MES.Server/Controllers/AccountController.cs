using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MES.Server.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly JwtHelper _jwtHelper;

    public AccountController(JwtHelper jwtHelper)
    {
        _jwtHelper = jwtHelper;
    }

    [HttpGet]
    public ActionResult<string> GetToken()
    {
        return _jwtHelper.CreateToken();
    }

    [Authorize]
    [HttpGet]
    public ActionResult<string> GetTest()
    {
        return "Test Authorize";
    }
}
