using System.Linq;
using System.Reflection;
using System.Windows;
using MES.Client.Core.Contracts.Views;
using MES.Client.Core.Interfaces;
using MES.WPF.Client.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Windows.Tools.Controls;

namespace MES.WPF.Client.Helpers;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 自动注册 Views 和 ViewModels
    /// </summary>
    /// <param name="services">DI 容器</param>
    /// <param name="assembly">要扫描的程序集</param>
    /// <param name="autoBindDataContext">是否自动为 View 设置 DataContext</param>
    public static IServiceCollection AddViewsAndViewModels(
        this IServiceCollection services,
        Assembly?               assembly,
        bool                    autoBindDataContext = false)
    {
        var types = assembly?.GetTypes();

        if (types == null)
        {
            return services;
        }

        // 找到所有Page
        var pages = types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRegisterPage<>)))
            .Where(t => t.IsClass && !t.IsAbstract && (t.Name.EndsWith("Page") || t.Name.EndsWith("View") || t.Name.EndsWith("Window")))
            .ToList();
        pages.ForEach(page => services.AddTransient(page));
        
        foreach (var page in pages)
        {
            var vmType = page.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRegisterPage<>))
                .GetGenericArguments()[0];

            services.AddTransient(vmType);

            // 注册委托代理，自动绑定
            if (autoBindDataContext)
            {
                services.AddTransient(page, provider =>
                {
                    var viewInstance = ActivatorUtilities.CreateInstance(provider, page);

                    if (viewInstance is FrameworkElement fe)
                    {
                        fe.DataContext = provider.GetRequiredService(vmType);
                    }

                    return viewInstance;
                });
            }
        }
        
        return services;
    }
}
