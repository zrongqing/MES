using App.AppServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.AppServices.Extensions;

public static class AppServersExtension
{
    /// <summary>
    /// 添加API的基础服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppServers(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetSection("AppServerConfig").GetSection("BaseUrl").Value! ;

        services.AddHttpClient<IApiClient,ApiClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddTransient<UserServer>();
        
        // services.AddHttpClient<IApiService, ApiService>(client =>
        // {
        //     client.BaseAddress = new Uri(baseUrl);
        //     client.DefaultRequestHeaders.Add("Accept", "application/json");
        // });
        
        return services;
    }
}