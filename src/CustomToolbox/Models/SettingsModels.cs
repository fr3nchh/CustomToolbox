namespace CustomToolbox.Models;

public class ToggleSetting
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string RegistryKey { get; set; } = "";
    public string RegistryValue { get; set; } = "";
    public int OnValue { get; set; } = 1;
    public int OffValue { get; set; } = 0;
    public bool DefaultValue { get; set; }
    public bool IsCurrentValue { get; set; }
    public string Category { get; set; } = "";
    public bool RequiresAdmin { get; set; }
    public bool RequiresReboot { get; set; }
    public string? ScriptOn { get; set; }
    public string? ScriptOff { get; set; }
    public bool MatchesSearch { get; set; } = true;
}

public class SelectionSetting
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string RegistryKey { get; set; } = "";
    public string RegistryValue { get; set; } = "";
    public List<SelectionOption> Options { get; set; } = new();
    public string CurrentValue { get; set; } = "";
    public string Category { get; set; } = "";
    public bool RequiresAdmin { get; set; }
    public bool MatchesSearch { get; set; } = true;
}

public class SelectionOption
{
    public string Label { get; set; } = "";
    public string Value { get; set; } = "";
    public string? Script { get; set; }
}

public class AppxPackage
{
    public string Name { get; set; } = "";
    public string PackageFullName { get; set; } = "";
    public bool IsRemovable { get; set; }
    public bool IsInstalled { get; set; }
    public string Description { get; set; } = "";
    public string Category { get; set; } = "Autre";
}

public class OptimizationCategory
{
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
    public List<ToggleSetting> ToggleSettings { get; set; } = new();
    public List<SelectionSetting> SelectionSettings { get; set; } = new();
}
