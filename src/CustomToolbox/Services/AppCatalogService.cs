using System.IO;
using System.Reflection;
using System.Text.Json;
using CustomToolbox.Models;

namespace CustomToolbox.Services;

public class AppCatalogService
{
    private static AppCatalog? _cached;

    public async Task<AppCatalog> LoadCatalog()
    {
        if (_cached != null) return _cached;

        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CustomToolbox.Resources.apps.json";
            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                return new AppCatalog();

            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _cached = JsonSerializer.Deserialize<AppCatalog>(json, options) ?? new AppCatalog();
            return _cached;
        }
        catch
        {
            return new AppCatalog();
        }
    }
}
