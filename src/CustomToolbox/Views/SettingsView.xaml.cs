using System.Windows;
using System.Windows.Controls;
using CustomToolbox.Services;

namespace CustomToolbox.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void OnCheckWinget_Click(object sender, RoutedEventArgs e)
    {
        bool installed = WingetBootstrap.IsWingetInstalled();
        string message = installed
            ? "Winget est installé sur votre système."
            : "Winget n'est pas installé. Il sera installé automatiquement lors de la première installation d'une application.";

        MessageBox.Show(message, "Winget", MessageBoxButton.OK,
            installed ? MessageBoxImage.Information : MessageBoxImage.Warning);
    }
}
