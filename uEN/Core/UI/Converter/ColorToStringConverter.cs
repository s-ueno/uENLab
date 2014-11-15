using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace uEN.UI
{
    public class ColorToStringConverter : IValueConverter
    {
        public static IEnumerable<Color> ListColors()
        {
            foreach (var each in dic.Keys)
            {
                yield return each;
            }
        }

        static ColorToStringConverter()
        {
            var pis = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (var p in pis)
            {
                try
                {
                    var c = (Color)p.GetValue(null, null);
                    dic[c] = p.Name;
                }
                catch
                {
                }
            }
            dic[(Color)ColorConverter.ConvertFromString("#307EB8")] = "深竹月";
        }
        private static readonly Dictionary<Color, string> dic = new Dictionary<Color, string>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = value as Color?;
            if (!color.HasValue)
                return null;

            if (dic.ContainsKey(color.Value))
            {
                return dic[color.Value];
            }
            return (value ?? string.Empty).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
