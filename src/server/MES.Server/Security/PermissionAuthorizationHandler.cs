using Microsoft.AspNetCore.Authorization;

namespace MES.Server.Security;

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
