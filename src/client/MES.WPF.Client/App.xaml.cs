using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using MES.Client.Core.Contracts.Services;
using MES.WPF.Client.Contracts.Services;
using MES.WPF.Client.Contracts.Views;
using MES.WPF.Client.Helpers;
using MES.WPF.Client.Models;
using MES.WPF.Client.Services;
using MES.WPF.Client.ViewModels;
using MES.WPF.Client.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace MES.WPF.Client
{
    // For more inforation about application lifecyle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

    // WPF UI elements use language en-US by default.
    // If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
    // Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
    public partial class App : Application
    {
        private IHost? _host;

        public T? GetService<T>()
            where T : class
            => _host?.Services.GetService(typeof(T)) as T;

        public App()
        {
            // Add your Syncfusion license key for WPF platform with corresponding Syncfusion NuGet version referred in project. For more information about license key see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
            // Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Add your license key here"); 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZedHVXRmNeVUVyXEJWYEk=");
            
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // todo 启动前事件
            base.OnStartup(e);
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

                // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
                _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(e.Args);
                    })
                    .ConfigureLogging(logging =>
                    {
                        // 添加日志
                        logging.ClearProviders();
                        logging.AddNLog();  // 会自动从 appsettings.json 获取配置
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

                await _host.StartAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // TODO WTS: Register your services, viewmodels and pages here

            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Services
            services.AddSingleton<IWindowManagerService, WindowManagerService>();
            services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
            services.AddSingleton<ISystemService, SystemService>();
            services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            // auto add views and viewmodels
            services.AddViewsAndViewModels(assembly: Assembly.GetAssembly(this.GetType()));

            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();
            
            services.AddTransient<IShellDialogWindow, ShellDialogWindow>();
            services.AddTransient<ShellDialogViewModel>();

            services.AddTransient<KanbanViewModel>();
            services.AddTransient<KanbanPage>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<PropertyGridViewModel>();
            services.AddTransient<PropertyGridPage>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<TileViewViewModel>();
            services.AddTransient<TileViewPage>();

            services.AddTransient<TreeGridViewModel>();
            services.AddTransient<TreeGridPage>();

            services.AddTransient<TreeViewViewModel>();
            services.AddTransient<TreeViewPage>();
            
            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                await _host?.StopAsync()!;
                _host.Dispose();
                _host = null;
            }
            catch (Exception exception)
            {
                // TODO handle exception
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
            
            NLog.LogManager.GetCurrentClassLogger().Error(e.Exception, "Unhandled exception");
        }
    }
}
