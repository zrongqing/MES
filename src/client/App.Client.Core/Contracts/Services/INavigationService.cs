using System.Windows.Controls;

namespace App.Client.Core.Contracts.Services;

/// <summary>
/// 导航服务接口
/// </summary>
public interface INavigationService
{
    event EventHandler<string> Navigated;

    bool CanGoBack { get; }

    void Initialize(Frame shellFrame);

    bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

    void GoBack();

    void UnsubscribeNavigation();

    void CleanNavigation();
}