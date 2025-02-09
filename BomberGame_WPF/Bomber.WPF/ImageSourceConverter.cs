using Bomber.WPF.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bomber.WPF;

public class ImageSourceConverter : IValueConverter
{
    #region Methods
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FieldType fieldType)
        {
            // Return the image source based on the field type, from the resources
            return Application.Current.Resources[$"{fieldType.ToString().ToLower()}Image"];
        }

        return null; // Return null if the value is not of type FieldType
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting from ImageSource to FieldType is not supported.");
    }
    #endregion
}
