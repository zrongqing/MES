using System.Windows.Controls;

using MES.WPF.Client.ViewModels;

namespace MES.WPF.Client.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
