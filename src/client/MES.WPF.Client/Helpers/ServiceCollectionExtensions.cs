using System.Linq;
using System.Reflection;
using System.Windows;
using MES.Client.Core.Interfaces;
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
        Assembly?               assembly,
        bool                    autoBindDataContext = false)
    {
        var types = assembly?.GetTypes();

        if (types == null)
        {
            return services;
        }

        // 找出所有 ViewModel
        var viewModels = types
            .Where(t => typeof(IRegisterViewModel).IsAssignableFrom(t))
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel"))
            .ToList();
        // 注册所有的ViewModel
        viewModels.ForEach(viewModel => services.AddTransient(viewModel));

        // 找到所有Page
        var pages = types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRegisterPage<>)))
            .Where(t => t.IsClass && !t.IsAbstract && (t.Name.EndsWith("Page") || t.Name.EndsWith("View") || t.Name.EndsWith("Window")))
            .ToList();
        pages.ForEach(page => services.AddTransient(page));

        if (!autoBindDataContext)
            return services;

        foreach (var vm in viewModels)
        {
            var viewName = vm.Name.Replace("ViewModel", "Page");

            var view = types
                .Where(t => typeof(IRegisterPage<>).IsAssignableFrom(t))
                .FirstOrDefault(v => v.Name == viewName);

            if (view == null)
            {
                viewName = vm.Name.Replace("ViewModel", "View");

                view = types
                    .Where(t => typeof(IRegisterPage<>).IsAssignableFrom(t))
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

        return services;
    }
}
