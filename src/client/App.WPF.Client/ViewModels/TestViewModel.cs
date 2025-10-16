using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace MES.WPF.Client.ViewModels;

public partial class TestViewModel : ObservableObject
{
    #region 用户登录测试

    [ObservableProperty]
    private string _username = "admin";
    
    [ObservableProperty]
    private string _password = "admin";

    [ObservableProperty]
    private bool _isAutoLogin = false;
    #endregion
    
    private readonly ILogger<TestViewModel> _logger;
    
    public TestViewModel(ILogger<TestViewModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 登录
    /// </summary>
    [RelayCommand]
    private void Login()
    {
        try
        {
            var app = (App)Application.Current;
            // var apiServer = app.GetService<ApiService>();
            
            
            _logger.LogError("LogError");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
        }
    }

    [RelayCommand]
    private void Register()
    {
        
    }
}