using MES.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace MES.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MESDbContext>();

        // 这里也可以注册仓储等
        // services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}