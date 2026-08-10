using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomToolbox;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void NavigateTo(string page)
    {
        foreach (var item in SidebarNav.Items)
        {
            if (item is ListBoxItem listBoxItem && listBoxItem.Tag?.ToString() == page)
            {
                SidebarNav.SelectedItem = listBoxItem;
                break;
            }
        }
    }

    private void OnNavigationChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SidebarNav.SelectedItem is not ListBoxItem item) return;

        var tag = item.Tag?.ToString();

        if (AppsView != null) AppsView.Visibility = tag == "Apps" ? Visibility.Visible : Visibility.Collapsed;
        if (OptimizeView != null) OptimizeView.Visibility = tag == "Optimize" ? Visibility.Visible : Visibility.Collapsed;
        if (CustomizeView != null) CustomizeView.Visibility = tag == "Customize" ? Visibility.Visible : Visibility.Collapsed;
        if (SettingsView != null) SettingsView.Visibility = tag == "Settings" ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
        else
        {
            DragMove();
        }
    }

    private void OnMinimize_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void OnMaximize_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void OnClose_Click(object sender, RoutedEventArgs e)
        => Close();
}
