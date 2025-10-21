﻿using System.Windows.Controls;
using MES.WPF.Client.Models;
using MES.WPF.Client.ViewModels;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;

namespace MES.WPF.Client.Views;

public partial class SettingsPage : Page
{
    public string item1 = string.Empty;
    public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        foreach (var theme in ThemeList.GetThemeList())
        {
            ComboBoxItemAdv item = new ComboBoxItemAdv() { Content = theme };
            ThemeSelection.Items.Add(item);
        }
        SfSkinManager.SetTheme(this, new Syncfusion.SfSkinManager.Theme(themeName));
    }
		
    private void ThemeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBoxAdv comboBoxAdv)
        {
            PaletteSelection.SelectedIndex = 0;
            if (comboBoxAdv.SelectedItem is ComboBoxItemAdv comboBoxItem)
            {
                var item = comboBoxItem.Content?.ToString();
                if (!string.IsNullOrWhiteSpace(item) && item.Contains(" "))
                {
                    item1 = item.Replace(" ", "");
                    SfSkinManager.SetTheme(this, new Syncfusion.SfSkinManager.Theme(item1));
                }
            }
        }

        if (PaletteSelection.SelectedIndex == -1)
        {
            PaletteSelection.SelectedIndex = 0;
        }
        else
        {
            PaletteSelection.SelectedIndex = PaletteSelection.SelectedIndex;
        }

        if (!string.IsNullOrWhiteSpace(item1) && item1 != "")
        {
            SfSkinManager.SetTheme(this, new Syncfusion.SfSkinManager.Theme(item1));
        }
    }
}