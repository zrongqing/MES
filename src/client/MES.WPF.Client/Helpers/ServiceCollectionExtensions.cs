using System.Linq;
using System.Reflection;
using System.Windows;
using MES.WPF.Client.Contracts.ViewModels;
using MES.WPF.Client.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;

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
        Assembly assembly,
        bool autoBindDataContext = false)
    {
        var types = assembly.GetTypes();

        // 找出所有 ViewModel
        var viewModels = types
            .Where(t => typeof(IRegisterViewModel).IsAssignableFrom(t))
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel"))
            .ToList();

        foreach (var vm in viewModels)
        {
            services.AddTransient(vm);

            // 自动绑定模式才去找 View
            if (autoBindDataContext)
            {
                var viewName = vm.Name.Replace("ViewModel", "Page");
                var view = types
                    .Where(t => typeof(IRegisterView).IsAssignableFrom(t))
                    .FirstOrDefault(v => v.Name == viewName);

                if (view == null)
                {
                    viewName = vm.Name.Replace("ViewModel", "View");
                    view = types
                        .Where(t => typeof(IRegisterView).IsAssignableFrom(t))
                        .FirstOrDefault(v => v.Name == viewName);
                }

                if (view != null)
                {
                    services.AddTransient(view, provider =>
                    {
                        var viewInstance = ActivatorUtilities.CreateInstance(provider, view);
                        if (viewInstance is FrameworkElement fe)
                        {
                            fe.DataContext = provider.GetRequiredService(vm);
                        }
                        return viewInstance;
                    });
                }
            }
        }

        // 如果不自动绑定，就把所有视图单独注册
        if (!autoBindDataContext)
        {
            var views = types
                .Where(t => typeof(IRegisterView).IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract &&
                            (t.Name.EndsWith("Page") || t.Name.EndsWith("View") || t.Name.EndsWith("Window")))
                .ToList();

            foreach (var view in views)
            {
                services.AddTransient(view);
            }
        }

        return services;
    }
}
