using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace uEN.UI.DataBinding
{
    public class SimpleValueConverter : IValueConverter
    {
        public Func<object, Type, object, CultureInfo, object> ConvertMethod { get; set; }
        public Func<object, Type, object, CultureInfo, object> ConvertBackMethod { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ConvertMethod != null)
                return ConvertMethod(value, targetType, parameter, culture);
            throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ConvertBackMethod != null)
                return ConvertBackMethod(value, targetType, parameter, culture);
            throw new NotImplementedException();
        }
    }
}
