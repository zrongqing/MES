using MES.Core.Common;
using MES.Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace MES.Server.Security;

public static class AuthorizationExtension
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorizationBuilder()
            .AddPolicy(Permissions.UserCreate, p => p.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserCreate)))
            .AddPolicy(Permissions.UserUpdate, p => p.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserUpdate)))
            .AddPolicy(Permissions.UserDelete, p => p.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserDelete)));

        return services;
    }
}