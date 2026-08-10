using System.Diagnostics;

namespace CustomToolbox.Services;

public static class PowerShellService
{
    public static async Task<(bool Success, string Output)> RunCommand(string command, bool asAdmin = false)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                UseShellExecute = asAdmin,
                Verb = asAdmin ? "runas" : "",
                RedirectStandardOutput = !asAdmin,
                RedirectStandardError = !asAdmin,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) return (false, "Failed to start process");

            if (!asAdmin)
            {
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                var output = await outputTask;
                var error = await errorTask;
                return (process.ExitCode == 0, string.IsNullOrEmpty(error) ? output : error);
            }

            await process.WaitForExitAsync();
            return (process.ExitCode == 0, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static async Task<bool> RunWingetInstall(string packageId)
    {
        var (success, _) = await RunCommand(
            $"Start-Process powershell -ArgumentList '-NoProfile -Command winget install --id {packageId} --silent --accept-package-agreements --accept-source-agreements' -Verb RunAs",
            asAdmin: false);
        return success;
    }

    public static async Task<bool> RemoveWindowsApp(string packageName)
    {
        var (success, _) = await RunCommand(
            $"Get-AppxPackage -Name '{packageName}' | Remove-AppxPackage",
            asAdmin: true);
        return success;
    }

    public static async Task<bool> InstallWindowsCapability(string capabilityName)
    {
        var (success, _) = await RunCommand(
            $"Add-WindowsCapability -Online -Name '{capabilityName}'",
            asAdmin: true);
        return success;
    }

    public static async Task<bool> RemoveWindowsCapability(string capabilityName)
    {
        var (success, _) = await RunCommand(
            $"Remove-WindowsCapability -Online -Name '{capabilityName}'",
            asAdmin: true);
        return success;
    }

    public static async Task<List<string>> GetInstalledWindowsApps()
    {
        var (success, output) = await RunCommand(
            "Get-AppxPackage | Select-Object Name, PackageFullName | Format-Table -AutoSize");
        
        if (!success) return new List<string>();
        
        return output.Split('\n')
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("---") && !l.StartsWith("Name"))
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();
    }

    public static async Task<List<string>> GetWindowsCapabilities()
    {
        var (success, output) = await RunCommand(
            "Get-WindowsCapability -Online | Where-Object {$_.State -eq 'Installed'} | Select-Object Name, State | Format-Table -AutoSize");
        
        if (!success) return new List<string>();
        
        return output.Split('\n')
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("---") && !l.StartsWith("Name"))
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();
    }

    public static async Task<bool> SetPowerPlan(string planGuid)
    {
        var (success, _) = await RunCommand(
            $"powercfg /setactive {planGuid}",
            asAdmin: true);
        return success;
    }

    public static async Task<List<(string Name, string Guid)>> GetPowerPlans()
    {
        var (success, output) = await RunCommand("powercfg /list");
        if (!success) return new List<(string, string)>();

        var plans = new List<(string, string)>();
        foreach (var line in output.Split('\n'))
        {
            if (line.Contains("Power Scheme GUID"))
            {
                var parts = line.Split(':');
                if (parts.Length >= 2)
                {
                    var guidPart = parts[1].Trim();
                    var namePart = guidPart.Contains('(') ? guidPart.Split('(')[1].TrimEnd(')') : "Unknown";
                    var guid = guidPart.Contains('(') ? guidPart.Split('(')[0].Trim() : "";
                    plans.Add((namePart, guid));
                }
            }
        }
        return plans;
    }
}
