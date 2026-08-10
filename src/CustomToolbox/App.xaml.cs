using System.Diagnostics;
using System.IO;
using System.Windows;
using CustomToolbox.Services;

namespace CustomToolbox;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var logFile = Path.Combine(AppContext.BaseDirectory, "crash.log");

        DispatcherUnhandledException += (_, args) =>
        {
            var ex = args.Exception;
            while (ex.InnerException != null) ex = ex.InnerException;
            var msg = $"[{DateTime.Now}] {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n";
            File.AppendAllText(logFile, msg);
            MessageBox.Show($"{ex.Message}\n\nSee crash.log for details.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        try
        {
            await WingetBootstrap.EnsureWingetAvailable();
        }
        catch (Exception ex)
        {
            File.AppendAllText(logFile, $"[{DateTime.Now}] Startup: {ex.Message}\n{ex.StackTrace}\n");
            MessageBox.Show($"Erreur au démarrage: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
