using System.Globalization;
using System.Windows.Data;

namespace CustomToolbox.Converters;

public class CategoryToIconConverter : IValueConverter
{
    private static readonly Dictionary<string, string> CategoryIcons = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Navigateurs", "🌐" },
        { "Communication", "💬" },
        { "Musique", "🎵" },
        { "Musique & Audio", "🎵" },
        { "Vidéo", "🎬" },
        { "Vidéo & Streaming", "🎬" },
        { "Jeux", "🎮" },
        { "Développement", "💻" },
        { "Utilitaires", "🔧" },
        { "Sécurité", "🛡️" }
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string category)
        {
            return CategoryIcons.TryGetValue(category, out var icon) ? icon : "📦";
        }
        return "📦";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
