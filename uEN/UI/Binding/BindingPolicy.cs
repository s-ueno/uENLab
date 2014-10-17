using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace uEN.UI.Binding
{

    public class BindingPolicy
    {
        private static readonly bool validatesOnDataErrors =
            BizUtils.AppSettings("BindingPolicy.ValidatesOnDataErrors", true);
        private static readonly bool validatesOnExceptions =
            BizUtils.AppSettings("BindingPolicy.ValidatesOnExceptions", true);
        private static readonly bool validatesOnNotifyDataErrors =
            BizUtils.AppSettings("BindingPolicy.ValidatesOnNotifyDataErrors", true);

        public BindingPolicy()
        {
            ValidatesOnDataErrors = validatesOnDataErrors;
            ValidatesOnExceptions = validatesOnExceptions;
            ValidatesOnNotifyDataErrors = validatesOnNotifyDataErrors;
            BindingMode = System.Windows.Data.BindingMode.Default;
        }

        public string StringFormat { get; set; }
        public BindingMode BindingMode { get; set; }
        public UpdateSourceTrigger? UpdateSourceTrigger { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public bool ValidatesOnNotifyDataErrors { get; set; }
        public IValueConverter Converter { get; set; }
    }
}
