using System.Windows.Controls;
using MES.Client.Core.Contracts.Views;
using MES.Client.Core.Interfaces;
using MES.WPF.Client.Contracts.Views;
using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Views;

public partial class KingdeePage : Page, IRegisterPage<KingdeeViewModel>
{
    public KingdeePage(KingdeeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public IPageMetadata PageMetadata { get; }
}

