using System.Collections.ObjectModel;
using CustomToolbox.Services;
using CustomToolbox.Models;

namespace CustomToolbox.ViewModels;

public class OptimizeViewModel : BaseViewModel
{
    public ObservableCollection<OptimizationCategory> Categories { get; } = new();
    
    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set { SetProperty(ref _searchText, value); FilterSettings(); }
    }

    private OptimizationCategory? _selectedCategory;
    public OptimizationCategory? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public OptimizeViewModel()
    {
        _ = LoadSettings();
    }

    private async Task LoadSettings()
    {
        // UAC Settings
        var uac = new OptimizationCategory { Name = "UAC (Contrôle de compte d'utilisateur)", Icon = "🛡️" };
        uac.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver UAC",
            Description = "Désactive les notifications de contrôle de compte d'utilisateur",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
            RegistryValue = "EnableLUA",
            OffValue = 0,
            OnValue = 1,
            RequiresAdmin = true,
            RequiresReboot = true
        });
        uac.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Notifications UAC en mode administrateur",
            Description = "Ne pas prompter les administrateurs pour l'élévation",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
            RegistryValue = "ConsentPromptBehaviorAdmin",
            OffValue = 0,
            OnValue = 5,
            RequiresAdmin = true
        });

        // Privacy Settings
        var privacy = new OptimizationCategory { Name = "Confidentialité", Icon = "🔒" };
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver la télémétrie",
            Description = "Désactive l'envoi de données de diagnostic à Microsoft",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
            RegistryValue = "AllowTelemetry",
            OffValue = 0,
            OnValue = 1,
            RequiresAdmin = true
        });
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver l'ID publicitaire",
            Description = "Désactive l'identifiant publicitaire unique",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
            RegistryValue = "Enabled",
            OffValue = 0,
            OnValue = 1
        });
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver le suivi de lancement d'applications",
            Description = "Empêche Windows de suivre quelles applications vous lancez",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "Start_TrackProgs",
            OffValue = 0,
            OnValue = 1
        });
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver la localisation",
            Description = "Désactive la géolocalisation",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location",
            RegistryValue = "Value",
            OffValue = 0,
            OnValue = 1
        });
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver Cortana",
            Description = "Désactive l'assistant Cortana",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
            RegistryValue = "AllowCortana",
            OffValue = 0,
            OnValue = 1,
            RequiresAdmin = true
        });
        privacy.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver le microphone par défaut",
            Description = "Désactive l'accès au microphone pour toutes les apps",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone",
            RegistryValue = "Value",
            OffValue = 0,
            OnValue = 1
        });

        // Gaming & Performance
        var gaming = new OptimizationCategory { Name = "Jeux & Performance", Icon = "🎮" };
        gaming.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Game Mode",
            Description = "Active le mode jeux pour optimiser les performances",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar",
            RegistryValue = "AllowAutoGameMode",
            OnValue = 1,
            OffValue = 0
        });
        gaming.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver Game Bar",
            Description = "Désactive la barre de jeux Windows",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
            RegistryValue = "AppCaptureEnabled",
            OffValue = 0,
            OnValue = 1
        });
        gaming.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver Nagle's Algorithm",
            Description = "Réduit la latence réseau pour les jeux en ligne",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\*",
            RegistryValue = "TcpAckFrequency",
            OnValue = 1,
            OffValue = 0,
            RequiresAdmin = true
        });
        gaming.SelectionSettings.Add(new SelectionSetting
        {
            Name = "Plan d'alimentation",
            Description = "Sélectionnez le plan d'alimentation optimal",
            Options = new List<SelectionOption>
            {
                new() { Label = "Équilibré", Value = "381b4222-f694-41f0-9685-ff5bb260df2e" },
                new() { Label = "Performances élevées", Value = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c" },
                new() { Label = "Économie d'énergie", Value = "a1841308-3541-4fab-bc81-f71556f20b4a" },
                new() { Label = "Performances ultimes", Value = "e9a42b02-d5df-448d-aa00-03f14749eb61" }
            }
        });

        // Windows Update
        var updates = new OptimizationCategory { Name = "Windows Update", Icon = "🔄" };
        updates.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver les mises à jour automatiques",
            Description = "Empêche Windows de télécharger et installer les mises à jour automatiquement",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU",
            RegistryValue = "NoAutoUpdate",
            OnValue = 1,
            OffValue = 0,
            RequiresAdmin = true
        });
        updates.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver les mises à jour de pilotes",
            Description = "Empêche Windows Update de installer les pilotes automatiquement",
            RegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate",
            RegistryValue = "ExcludeWUDriversInQualityUpdate",
            OnValue = 1,
            OffValue = 0,
            RequiresAdmin = true
        });

        // Sound Settings
        var sound = new OptimizationCategory { Name = "Son", Icon = "🔊" };
        sound.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver les sons système",
            Description = "Désactive les effets sonores de Windows",
            RegistryKey = @"HKEY_CURRENT_USER\AppEvents\Schemes",
            RegistryValue = ".None",
            OffValue = 0,
            OnValue = 1
        });

        // Notifications
        var notif = new OptimizationCategory { Name = "Notifications", Icon = "🔔" };
        notif.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver les notifications",
            Description = "Désactive toutes les notifications Windows",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications",
            RegistryValue = "ToastEnabled",
            OffValue = 0,
            OnValue = 1
        });
        notif.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver les notifications d'applications",
            Description = "Désactive les notifications des applications en arrière-plan",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications",
            RegistryValue = "LockScreenToastEnabled",
            OffValue = 0,
            OnValue = 1
        });
        notif.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Masquer les badges sur les apps",
            Description = "Masque les pastilles de notification sur les icônes de la barre des tâches",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "TaskbarBadges",
            OffValue = 0,
            OnValue = 1
        });

        // Load current values
        foreach (var cat in new[] { uac, privacy, gaming, updates, sound, notif })
        {
            foreach (var toggle in cat.ToggleSettings)
            {
                toggle.IsCurrentValue = toggle.RegistryKey.Contains("HKEY_CURRENT_USER")
                    ? RegistryService.GetCurrentUserBoolValue(toggle.RegistryKey, toggle.RegistryValue, toggle.DefaultValue)
                    : RegistryService.GetBoolValue(toggle.RegistryKey, toggle.RegistryValue, toggle.DefaultValue);
            }
            Categories.Add(cat);
        }

        SelectedCategory = Categories.FirstOrDefault();
    }

    public async Task ApplyToggle(ToggleSetting setting, bool newValue)
    {
        if (setting.ScriptOn != null && newValue)
        {
            await PowerShellService.RunCommand(setting.ScriptOn, setting.RequiresAdmin);
        }
        else if (setting.ScriptOff != null && !newValue)
        {
            await PowerShellService.RunCommand(setting.ScriptOff, setting.RequiresAdmin);
        }
        else
        {
            var value = newValue ? setting.OnValue : setting.OffValue;
            if (setting.RegistryKey.Contains("HKEY_CURRENT_USER"))
                RegistryService.SetCurrentUserValue(setting.RegistryKey, setting.RegistryValue, value);
            else
                RegistryService.SetValue(setting.RegistryKey, setting.RegistryValue, value);
        }
        setting.IsCurrentValue = newValue;
    }

    public async Task ApplySelection(SelectionSetting setting, string value)
    {
        if (!string.IsNullOrEmpty(setting.RegistryKey))
        {
            var intValue = int.TryParse(value, out var v) ? v : 0;
            if (setting.RegistryKey.Contains("HKEY_CURRENT_USER"))
                RegistryService.SetCurrentUserValue(setting.RegistryKey, setting.RegistryValue, intValue);
            else
                RegistryService.SetValue(setting.RegistryKey, setting.RegistryValue, intValue);
        }

        var option = setting.Options.FirstOrDefault(o => o.Value == value);
        if (option?.Script != null)
        {
            await PowerShellService.RunCommand(option.Script, setting.RequiresAdmin);
        }

        setting.CurrentValue = value;
    }

    private void FilterSettings()
    {
        var query = SearchText?.Trim() ?? "";
        var hasQuery = !string.IsNullOrEmpty(query);

        foreach (var cat in Categories)
        {
            foreach (var toggle in cat.ToggleSettings)
            {
                toggle.MatchesSearch = !hasQuery ||
                    toggle.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    toggle.Description.Contains(query, StringComparison.OrdinalIgnoreCase);
            }
            foreach (var sel in cat.SelectionSettings)
            {
                sel.MatchesSearch = !hasQuery ||
                    sel.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    sel.Description.Contains(query, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    public bool IsSettingVisible(ToggleSetting setting) => setting.MatchesSearch;

    public bool IsSelectionVisible(SelectionSetting setting) => setting.MatchesSearch;
}
