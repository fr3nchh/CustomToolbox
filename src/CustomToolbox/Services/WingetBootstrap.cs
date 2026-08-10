using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace CustomToolbox.Services;

public static class WingetBootstrap
{
    public static readonly string WingetPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"Microsoft\WindowsApps\winget.exe");

    public static async Task EnsureWingetAvailable()
    {
        if (!IsWingetInstalled())
        {
            await InstallWinget();
        }
    }

    public static bool IsWingetInstalled()
    {
        return File.Exists(WingetPath);
    }

    private static async Task InstallWinget()
    {
        try
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "winget.msixbundle");
            var url = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle";

            using var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(tempPath, bytes);

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"Add-AppxPackage -Path \"{tempPath}\" -ForceApplicationShutdown",
                Verb = "runas",
                UseShellExecute = true
            });

            if (process != null)
                await process.WaitForExitAsync();

            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur installation winget: {ex.Message}");
        }
    }
}
