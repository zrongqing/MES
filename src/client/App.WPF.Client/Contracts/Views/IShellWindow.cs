using System.Windows.Controls;

namespace MES.WPF.Client.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}