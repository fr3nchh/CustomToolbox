using System.Text.Json.Serialization;

namespace CustomToolbox.Models;

public class Category
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("apps")]
    public List<AppInfo> Apps { get; set; } = new();
}
