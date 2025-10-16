using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using App.Client.Core.Contracts.Views;
using App.Client.Core.Interfaces;
using App.Client.Core.Models;
using MES.WPF.Client.ViewModels;
using MES.WPF.Client.Properties;

namespace MES.WPF.Client.Views;

public partial class MainPage : Page, IRegisterPage<MainViewModel>
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public IPageMetadata PageMetadata { get; } = new PageMetadata()
    {
        Label = MES.WPF.Client.Properties.Resources.ShellMainPage,
        Path = new Path()
        {
            Width = 15,
            Height = 15,
            Data = Geometry.Parse("M28.414 4H7V44H39V14.586ZM29 7.414 35.586 14H29ZM9 42V6H27V16H37V42Z"),
            Fill = new SolidColorBrush(Colors.Black),
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            Stretch = Stretch.Fill,
        },
        TargetType = typeof(MainViewModel)
    };
}