using System.Globalization;

namespace ICSProject.MAUI.Converters;

public class BoolToSortDirectionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "↑ Ascending" : "↓ Descending";
        }
    
        return "↑ Ascending";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}