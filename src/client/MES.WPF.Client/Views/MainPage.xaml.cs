using System.Windows.Controls;
using MES.Client.Core.Interfaces;
using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Views
{
    public partial class MainPage : Page, IRegisterPage<MainViewModel>
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
