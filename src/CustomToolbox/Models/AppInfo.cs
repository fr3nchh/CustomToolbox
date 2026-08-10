using System.Text.Json.Serialization;

namespace CustomToolbox.Models;

public class AppInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("packageId")]
    public string PackageId { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; } = string.Empty;

    [JsonPropertyName("website")]
    public string Website { get; set; } = string.Empty;

    [JsonPropertyName("docs")]
    public string DocumentationUrl { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsInstalled { get; set; }

    [JsonIgnore]
    public int IconDisplaySize { get; set; } = 40;

    [JsonIgnore]
    public InstallStatus Status { get; set; } = InstallStatus.NotInstalled;
}

public enum InstallStatus
{
    NotInstalled,
    Installing,
    Installed,
    Error
}
