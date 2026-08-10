using System.Windows;
using CustomToolbox.Services;

namespace CustomToolbox;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Vérifier et installer winget si nécessaire au démarrage
        await WingetBootstrap.EnsureWingetAvailable();
    }
}
