using System.Text.Json.Serialization;

namespace CustomToolbox.Models;

public class AppCatalog
{
    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; } = new();
}
