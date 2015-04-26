using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI.DataBinding
{
    public class BindingStringFormatAttribute : Attribute
    {
        public BindingStringFormatAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; private set; }
    }
}
