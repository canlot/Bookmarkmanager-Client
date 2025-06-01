using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Bookmark_Manager_Client.Converters
{
    [ValueConversion(typeof(string), typeof(DrawingImage))]
    public class IconNameToDrawingImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || parameter.GetType() != typeof(ResourceDictionary) )
                return null;
            ResourceDictionary iconsDictionary = parameter as ResourceDictionary;
            if (value == null) return null;
            if (value is string)
            {
                var name = (string)value + "Icon";
                var icon = (IconElement)iconsDictionary[name];
                if (icon == null)
                {
                    icon = (IconElement)iconsDictionary["folder_openIcon"];
                    return icon.Icon;
                }
                else
                    return icon.Icon;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
