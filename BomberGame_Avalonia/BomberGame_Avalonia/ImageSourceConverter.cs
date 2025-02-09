using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BomberGame_Avalonia.ViewModels;
using System;
using System.Globalization;

namespace BomberGame_Avalonia
{
    public class ImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is FieldType fieldType)
            {
                // Map FieldType to resource URI
                string imagePath = fieldType switch
                {
                    FieldType.PLAYER => "avares://BomberGame_Avalonia/Assets/Images/player.jpg",
                    FieldType.ENEMY => "avares://BomberGame_Avalonia/Assets/Images/enemy.jpg",
                    FieldType.BOMB => "avares://BomberGame_Avalonia/Assets/Images/bomb.jpg",
                    FieldType.WALL => "avares://BomberGame_Avalonia/Assets/Images/wall.jpg",
                    FieldType.EXPLOSION => "avares://BomberGame_Avalonia/Assets/Images/explosion.png",
                    FieldType.EMPTY => "avares://BomberGame_Avalonia/Assets/Images/empty.jpg",
                    _ => "avares://BomberGame_Avalonia/Assets/Images/empty.jpg",
                };

                try
                {
                    return new Bitmap(AssetLoader.Open(new Uri(imagePath)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image for FieldType {fieldType}: {ex.Message}");
                }
            }

            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}
