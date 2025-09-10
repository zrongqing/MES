using System.Windows.Controls;
using MES.WPF.Client.ViewModels;
using Syncfusion.SfSkinManager;
namespace MES.WPF.Client.Views
{
    public partial class TreeGridPage : Page
    {
		public string themeName = App.Current.Properties["Theme"]?.ToString()!= null? App.Current.Properties["Theme"]?.ToString(): "Windows11Light";
        public TreeGridPage(TreeGridViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
			SfSkinManager.SetTheme(this, new Theme(themeName));
        }
    }
}
