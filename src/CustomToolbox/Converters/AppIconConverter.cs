using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using CustomToolbox.Services;

namespace CustomToolbox.Converters;

public class AppIconConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string iconUrl && !string.IsNullOrEmpty(iconUrl))
            return IconService.GetIconFromUrl(iconUrl);
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
