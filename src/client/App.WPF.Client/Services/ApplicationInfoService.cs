using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using App.Client.Core.Contracts.Services;
using App.Client.Core.Models;
using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public ApplicationInfoService()
    {
    }

    public Version GetVersion()
    {
        // Set the app version in DevTool.Client > Properties > Package > PackageVersion
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        return new Version(version);
    }

    public List<PageMetadata> GetNavigationPaneItems()
    {
        var menuItems = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(PageMetadata).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (PageMetadata)Activator.CreateInstance(t)!;
                return new PageMetadata
                {
                    Label = instance.Label,
                    Path = instance.Path,
                    TargetType = instance.TargetType,
                };
            }).ToList();

        return menuItems;
    }
}