using System.Windows.Controls;
using MES.WPF.Client.Contracts.Views;
using MES.WPF.Client.ViewModels;
using Syncfusion.Windows.Shared;

namespace MES.WPF.Client.Views;

public partial class ShellDialogWindow : ChromelessWindow, IShellDialogWindow
{
    public ShellDialogWindow(ShellDialogViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetResult = OnSetResult;
        DataContext = viewModel;
    }

    public Frame GetDialogFrame()
        => dialogFrame;

    private void OnSetResult(bool? result)
    {
        DialogResult = result;
        Close();
    }
}