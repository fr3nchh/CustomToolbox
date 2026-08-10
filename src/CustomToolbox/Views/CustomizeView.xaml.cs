using System.Windows;
using System.Windows.Controls;
using CustomToolbox.Models;
using CustomToolbox.ViewModels;

namespace CustomToolbox.Views;

public partial class CustomizeView : UserControl
{
    public CustomizeView()
    {
        InitializeComponent();
    }

    private CustomizeViewModel ViewModel => (CustomizeViewModel)DataContext;

    private async void OnToggleChecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox cb && cb.Tag is ToggleSetting setting)
        {
            await ViewModel.ApplyToggle(setting, true);
        }
    }

    private async void OnToggleUnchecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox cb && cb.Tag is ToggleSetting setting)
        {
            await ViewModel.ApplyToggle(setting, false);
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox combo && combo.Tag is SelectionSetting setting && combo.SelectedValue != null)
        {
            await ViewModel.ApplySelection(setting, combo.SelectedValue.ToString()!);
        }
    }
}
