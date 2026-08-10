using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace CustomToolbox.Services;

public static class IconService
{
    private static readonly HttpClient _httpClient = new();
    private static readonly ConcurrentDictionary<string, BitmapImage> _urlCache = new();
    private static readonly string _cacheDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CustomToolbox", "IconCache");

    static IconService()
    {
        if (!Directory.Exists(_cacheDir))
            Directory.CreateDirectory(_cacheDir);
    }

    public static async Task<BitmapImage?> GetIconAsync(string packageName, string iconUrl)
    {
        if (_urlCache.TryGetValue(iconUrl, out var cached))
            return cached;

        var localPath = Path.Combine(_cacheDir, $"{SanitizeFileName(packageName)}.png");
        if (File.Exists(localPath))
            return LoadFromDisk(localPath, iconUrl);

        if (!string.IsNullOrEmpty(iconUrl))
        {
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(iconUrl);
                await File.WriteAllBytesAsync(localPath, bytes);
                return LoadFromDisk(localPath, iconUrl);
            }
            catch { }
        }

        return null;
    }

    public static BitmapImage? GetIconFromUrl(string iconUrl)
    {
        if (string.IsNullOrEmpty(iconUrl)) return null;

        if (_urlCache.TryGetValue(iconUrl, out var cached))
            return cached;

        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(iconUrl, UriKind.Absolute);
            image.DecodePixelWidth = 32;
            image.EndInit();
            image.Freeze();
            _urlCache[iconUrl] = image;
            return image;
        }
        catch
        {
            return null;
        }
    }

    private static BitmapImage? LoadFromDisk(string path, string key)
    {
        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.DecodePixelWidth = 32;
            image.EndInit();
            image.Freeze();

            _urlCache[key] = image;
            return image;
        }
        catch { return null; }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
}
