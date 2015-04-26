using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace uEN.UI.DataBinding
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
            ValidationRules = new List<ValidationRule>();
        }

        public string StringFormat { get; set; }
        public BindingMode BindingMode { get; set; }
        public UpdateSourceTrigger? UpdateSourceTrigger { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public bool ValidatesOnNotifyDataErrors { get; set; }
        public IValueConverter Converter { get; set; }
        public int? Delay { get; set; }
        public bool? BindsDirectlyToSource { get; set; }
        public object FallbackValue { get; set; }
        public bool? IsAsync { get; set; }
        public bool? NotifyOnSourceUpdated { get; set; }
        public bool? NotifyOnTargetUpdated { get; set; }
        public bool? NotifyOnValidationError { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object TargetNullValue { get; set; }
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get; set; }
        public List<ValidationRule> ValidationRules { get; private set; }


    }
}
