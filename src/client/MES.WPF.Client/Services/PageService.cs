using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using MES.Client.Core.Contracts.Services;
using MES.Client.Core.Contracts.Views;
using MES.Client.Core.Interfaces;
using MES.WPF.Client.Helpers;
using MES.WPF.Client.ViewModels;
using MES.WPF.Client.Views;

namespace MES.WPF.Client.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private readonly IServiceProvider _serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Configure<KanbanViewModel, KanbanPage>();
            //Configure<MainViewModel, MainPage>();
            Configure<PropertyGridViewModel, PropertyGridPage>();
            Configure<SettingsViewModel, SettingsPage>();
            Configure<TileViewViewModel, TileViewPage>();
            Configure<TreeGridViewModel, TreeGridPage>();
            Configure<TreeViewViewModel, TreeViewPage>();

            AutoConfigureByInterface();
        }
        
        /// <summary>
        /// 通过接口自动注入
        /// </summary>
        private void AutoConfigureByInterface()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var pageTypes = assembly
                    .GetTypes()
                    .Where(t => typeof(Page).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var pageType in pageTypes)
                {
                    var interfaceType = pageType.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRegisterPage<>));

                    if (interfaceType != null)
                    {
                        var vmType = interfaceType.GetGenericArguments().First();
                        Configure(vmType, pageType);
                    }
                }
            }
        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }

            return pageType;
        }

        public Page GetPage(string key)
        {
            var pageType = GetPageType(key);
            return _serviceProvider.GetService(pageType) as Page;
        }

        private void Configure(Type viewModelType, Type pageType)
        {
            lock (_pages)
            {
                var key = viewModelType.FullName!;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }
                
                var type = pageType;
                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }
                
                _pages.Add(key, pageType);
            }
            
        }
        private void Configure<VM, V>()
            where VM : Observable
            where V : Page
        {
            lock (_pages)
            {
                var key = typeof(VM).FullName;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(V);
                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}
