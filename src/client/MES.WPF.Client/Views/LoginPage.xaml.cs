using System.Windows.Controls;
using MES.Client.Core.Contracts.Views;
using MES.Client.Core.Interfaces;
using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Views;

public partial class LoginPage : Page,IRegisterPage<LoginViewModel>
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public IPageMetadata PageMetadata { get; }
}

