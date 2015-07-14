using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    public class CustomUInt32Converter : UInt32Converter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string text = ((string)value).Trim();
                culture = CultureInfo.InvariantCulture;

                var formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
                return UInt32.Parse(text, NumberStyles.Number, formatInfo);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
