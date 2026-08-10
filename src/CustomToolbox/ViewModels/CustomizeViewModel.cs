using System.Collections.ObjectModel;
using CustomToolbox.Services;
using CustomToolbox.Models;

namespace CustomToolbox.ViewModels;

public class CustomizeViewModel : BaseViewModel
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

    public CustomizeViewModel()
    {
        _ = LoadSettings();
    }

    private Task LoadSettings()
    {
        // Theme Settings
        var theme = new OptimizationCategory { Name = "Thème & Apparence", Icon = "🎨" };
        theme.SelectionSettings.Add(new SelectionSetting
        {
            Name = "Mode de thème",
            Description = "Sélectionnez le thème Windows",
            Options = new List<SelectionOption>
            {
                new() { Label = "Sombre", Value = "0", Script = "Set-ItemProperty -Path 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize' -Name 'AppsUseLightTheme' -Value 0; Set-ItemProperty -Path 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize' -Name 'SystemUsesLightTheme' -Value 0" },
                new() { Label = "Clair", Value = "1", Script = "Set-ItemProperty -Path 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize' -Name 'AppsUseLightTheme' -Value 1; Set-ItemProperty -Path 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize' -Name 'SystemUsesLightTheme' -Value 1" }
            },
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
            RegistryValue = "AppsUseLightTheme"
        });
        theme.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Effets de transparence",
            Description = "Active/désactive les effets de transparence de Windows",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
            RegistryValue = "EnableTransparency",
            OnValue = 1,
            OffValue = 0
        });
        theme.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Effets de animation",
            Description = "Active/désactive les effets d'animation",
            RegistryKey = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics",
            RegistryValue = "MinAnimate",
            OnValue = 1,
            OffValue = 0
        });

        // Taskbar Settings
        var taskbar = new OptimizationCategory { Name = "Barre des tâches", Icon = "📊" };
        taskbar.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les icônes de la barre des tâches",
            Description = "Affiche toutes les icônes dans la zone de notification",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer",
            RegistryValue = "EnableAutoTray",
            OffValue = 0,
            OnValue = 1
        });
        taskbar.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Bouton de la barre des tâches à gauche",
            Description = "Positionne le bouton de la barre des tâches à gauche",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "TaskbarAl",
            OffValue = 0,
            OnValue = 1
        });
        taskbar.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les widgets",
            Description = "Affiche le bouton Widgets sur la barre des tâches",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "TaskbarDa",
            OnValue = 1,
            OffValue = 0
        });
        taskbar.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher le chat",
            Description = "Affiche le bouton Chat sur la barre des tâches",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "TaskbarMn",
            OnValue = 1,
            OffValue = 0
        });
        taskbar.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher le bouton Recherche",
            Description = "Affiche le bouton Recherche sur la barre des tâches",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search",
            RegistryValue = "SearchboxTaskbarMode",
            OnValue = 1,
            OffValue = 0
        });
        taskbar.SelectionSettings.Add(new SelectionSetting
        {
            Name = "Taille des icônes de la barre des tâches",
            Description = "Sélectionnez la taille des icônes",
            Options = new List<SelectionOption>
            {
                new() { Label = "Petit", Value = "0" },
                new() { Label = "Moyen (défaut)", Value = "1" },
                new() { Label = "Grand", Value = "2" }
            },
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "TaskbarSmallIcons"
        });

        // Start Menu Settings
        var startMenu = new OptimizationCategory { Name = "Menu Démarrer", Icon = "🪟" };
        startMenu.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les applications récentes",
            Description = "Affiche les applications récentes dans le menu Démarrer",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "Start_TrackDocs",
            OnValue = 1,
            OffValue = 0
        });
        startMenu.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les suggestions",
            Description = "Affiche les applications suggérées dans le menu Démarrer",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
            RegistryValue = "SystemPaneSuggestionsEnabled",
            OffValue = 0,
            OnValue = 1
        });
        startMenu.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les apps les plus utilisées",
            Description = "Affiche les applications les plus utilisées dans le menu Démarrer",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "Start_TrackProgs",
            OnValue = 1,
            OffValue = 0
        });

        // Explorer Settings
        var explorer = new OptimizationCategory { Name = "Explorateur de fichiers", Icon = "📁" };
        explorer.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les extensions de fichiers",
            Description = "Affiche les extensions de fichiers connues",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "HideFileExt",
            OffValue = 0,
            OnValue = 1
        });
        explorer.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher les fichiers cachés",
            Description = "Affiche les fichiers et dossiers cachés",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "Hidden",
            OnValue = 1,
            OffValue = 0
        });
        explorer.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Ouvrir l'Explorateur sur Ce PC",
            Description = "Ouvre l'Explorateur sur Ce PC au lieu de Accueil rapide",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            RegistryValue = "LaunchTo",
            OnValue = 1,
            OffValue = 0
        });
        explorer.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Afficher la barre d'adresse complète",
            Description = "Affiche le chemin complet dans la barre d'adresse",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CabinetState",
            RegistryValue = "FullPathAddress",
            OnValue = 1,
            OffValue = 0
        });
        explorer.ToggleSettings.Add(new ToggleSetting
        {
            Name = "Désactiver la barre de navigation récente",
            Description = "Masque les éléments récents dans la barre de navigation",
            RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Ribbon",
            RegistryValue = "QatItems",
            OffValue = 0,
            OnValue = 1
        });

        // Load current values
        foreach (var cat in new[] { theme, taskbar, startMenu, explorer })
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
        return Task.CompletedTask;
    }

    public async Task ApplyToggle(ToggleSetting setting, bool newValue)
    {
        var value = newValue ? setting.OnValue : setting.OffValue;
        if (setting.RegistryKey.Contains("HKEY_CURRENT_USER"))
            RegistryService.SetCurrentUserValue(setting.RegistryKey, setting.RegistryValue, value);
        else
            RegistryService.SetValue(setting.RegistryKey, setting.RegistryValue, value);
        setting.IsCurrentValue = newValue;
    }

    public async Task ApplySelection(SelectionSetting setting, string value)
    {
        var option = setting.Options.FirstOrDefault(o => o.Value == value);
        if (option?.Script != null)
        {
            await PowerShellService.RunCommand(option.Script);
        }
        else if (!string.IsNullOrEmpty(setting.RegistryKey))
        {
            var intValue = int.TryParse(value, out var v) ? v : 0;
            if (setting.RegistryKey.Contains("HKEY_CURRENT_USER"))
                RegistryService.SetCurrentUserValue(setting.RegistryKey, setting.RegistryValue, intValue);
            else
                RegistryService.SetValue(setting.RegistryKey, setting.RegistryValue, intValue);
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
}
