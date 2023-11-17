using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bookmark_Manager_Client.Converters
{
    [ValueConversion(typeof(uint), typeof(bool))]
    public class ParentIdToReadOnlyStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return null;
            if(value is uint)
            {
                var id = (uint)value;
                if(id == 0)
                    return true;
                else
                    return false;
            }
            return null;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
