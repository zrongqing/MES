using System.Windows.Controls;
using App.Client.Core.Contracts.Views;
using App.Client.Core.Interfaces;
using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Views;

/// <summary>
/// 测试页面
/// </summary>
public partial class TestPage : Page,IRegisterPage<TestViewModel>
{
    public TestPage(TestViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public IPageMetadata PageMetadata { get; }
}