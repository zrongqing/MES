using CommunityToolkit.Mvvm.ComponentModel;

namespace MES.Client.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}
