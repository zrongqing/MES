using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace MES.WPF.Client.ViewModels;

public partial class TestViewModel : ObservableObject
{
    #region 用户登录测试

    [ObservableProperty]
    private string _username = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;

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
            _logger.LogError("LogError");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
        }
    }
}
