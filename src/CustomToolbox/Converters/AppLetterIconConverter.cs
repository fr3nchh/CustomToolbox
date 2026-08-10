using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomToolbox.Converters;

public class AppLetterIconConverter : IValueConverter
{
    private static readonly Brush[] Colors = new Brush[]
    {
        new SolidColorBrush(Color.FromRgb(0x6C, 0x63, 0xFF)), // Violet
        new SolidColorBrush(Color.FromRgb(0x00, 0x96, 0x88)), // Teal
        new SolidColorBrush(Color.FromRgb(0xE9, 0x1E, 0x63)), // Pink
        new SolidColorBrush(Color.FromRgb(0xFF, 0x57, 0x22)), // Orange
        new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3)), // Blue
        new SolidColorBrush(Color.FromRgb(0x4, 0xCA, 0xF0)), // Cyan
        new SolidColorBrush(Color.FromRgb(0x9C, 0x27, 0xB0)), // Purple
        new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00)), // Amber
        new SolidColorBrush(Color.FromRgb(0x8B, 0xC3, 0x4A)), // Light Green
        new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36)), // Red
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string name && !string.IsNullOrEmpty(name))
        {
            var firstChar = name.Trim().ToUpperInvariant()[0];
            var index = firstChar % Colors.Length;
            return Colors[index];
        }
        return Colors[0];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class AppFirstLetterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string name && !string.IsNullOrEmpty(name))
        {
            return name.Trim().ToUpperInvariant()[0].ToString();
        }
        return "?";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
