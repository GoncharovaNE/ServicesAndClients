using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.IO;
using Avalonia.Platform;

namespace ServicesAndClients.Converters
{
    internal class ImageConvert : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null ? new Bitmap(AssetLoader.Open(new Uri("avares://ServicesAndClients/Assets/school_logo.ico"))) : new Bitmap(AssetLoader.Open(new Uri($"avares://ServicesAndClients/Assets/{value}")));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
