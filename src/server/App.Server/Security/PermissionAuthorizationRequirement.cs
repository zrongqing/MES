using Microsoft.AspNetCore.Authorization;

namespace App.Server.Security;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionAuthorizationRequirement(string permission)
    {
        Permission = permission;
    }
}