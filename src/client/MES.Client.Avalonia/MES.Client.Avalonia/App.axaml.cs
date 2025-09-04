using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using MES.Client.Avalonia.ViewModels;
using MES.Client.Avalonia.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MES.Client.Avalonia;

public partial class App : Application
{
    // 全局可访问的 Host 实例
    public static IHost Host { get; private set; } = null!;
    
    private IHost _host = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();

        return;
        
        // // 如果使用 CommunityToolkit，则需要用下面一行移除 Avalonia 数据验证。
        // // 如果没有这一行，数据验证将会在 Avalonia 和 CommunityToolkit 中重复。
        // BindingPlugins.DataValidators.RemoveAt(0);
        //
        // // 注册应用程序运行所需的所有服务
        // var collection = new ServiceCollection();
        //
        // // 创建通用主机
        // Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
        //     .ConfigureAppConfiguration(config =>
        //     {
        //         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //     })
        //     .ConfigureServices((context, services) =>
        //     {
        //         services.AddCommonServices();
        //         services.AddSingleton<MainViewModel>();
        //         services.AddSingleton<MainWindow>();
        //         services.AddSingleton<MainView>();
        //     })
        //     .ConfigureLogging(logging =>
        //     {
        //         logging.ClearProviders();
        //         logging.AddConsole();
        //     })
        //     .Build();
        //
        //
        // _host = Host;
        //
        // if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        // {
        //     // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
        //     // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
        //     DisableAvaloniaDataAnnotationValidation();
        //
        //     var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        //     mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
        //     desktop.MainWindow = mainWindow;
        //     
        //     // desktop.MainWindow = new MainWindow
        //     // {
        //     //     DataContext = new MainViewModel()
        //     // };
        // }
        // else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        // {
        //     var mainView = _host.Services.GetRequiredService<MainView>();
        //     mainView.DataContext = _host.Services.GetRequiredService<MainViewModel>();
        //     singleViewPlatform.MainView = mainView;
        //     
        //     // singleViewPlatform.MainView = new MainView
        //     // {
        //     //     DataContext = new MainViewModel()
        //     // };
        // }
        //
        // base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
