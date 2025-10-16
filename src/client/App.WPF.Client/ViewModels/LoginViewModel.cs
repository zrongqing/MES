using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace MES.WPF.Client.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isAutoLogin = false;
    
    private readonly ILogger<LoginViewModel> _logger;

    public LoginViewModel(ILogger<LoginViewModel> logger)
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