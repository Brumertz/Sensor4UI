using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class ColorToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString()));
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}