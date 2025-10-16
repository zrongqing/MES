using MES.Core.Common;
using MES.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MES.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    [Authorize(Permissions.UserCreate)]
    public ActionResult<string> UserCreate() => "UserCreate";

    [HttpGet]
    [Authorize(Permissions.UserUpdate)]
    public ActionResult<string> UserUpdate() => "UserUpdate";

    [HttpGet]
    [Authorize(Permissions.UserDelete)]
    public ActionResult<string> UserDelete() => "UserDelete";
}
