using System.Diagnostics;
using System.IO;

namespace CustomToolbox.Services;

public class WingetService
{
    private static readonly string WingetPath = WingetBootstrap.WingetPath;

    public async Task<bool> InstallApp(string packageId)
    {
        try
        {
            var result = await RunWinget($"install --id \"{packageId}\" --silent --accept-package-agreements --accept-source-agreements");
            return result.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UninstallApp(string packageId)
    {
        try
        {
            var result = await RunWinget($"uninstall --id \"{packageId}\" --silent");
            return result.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsInstalled(string packageId)
    {
        try
        {
            var result = await RunWinget($"list --id \"{packageId}\" --disable-interactivity");
            return result.ExitCode == 0 && result.Output.Contains(packageId, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<string>> SearchApps(string query)
    {
        try
        {
            var result = await RunWinget($"search \"{query}\" --disable-interactivity");
            return result.Output.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        catch
        {
            return new List<string>();
        }
    }

    private async Task<(int ExitCode, string Output)> RunWinget(string arguments)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = WingetPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process == null) return (-1, string.Empty);

            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            var output = await outputTask;
            var error = await errorTask;

            return (process.ExitCode, output + error);
        }
        catch
        {
            return (-1, string.Empty);
        }
    }
}
