using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace uEN.UI
{
    public class FontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fontFamily = value as FontFamily;
            if (fontFamily == null) return null;

            var jpKey = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name);
            if (fontFamily.FamilyNames.ContainsKey(jpKey))
            {
                var localName = fontFamily.FamilyNames[jpKey];
                if (!string.IsNullOrWhiteSpace(localName))
                {
                    return localName;
                }
            }
            return fontFamily.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
