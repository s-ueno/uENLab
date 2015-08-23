using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using uEN.Properties;
using uEN.UI.DataBinding;

namespace uEN.UI.Validation
{

    public class RequiredRuleAttribute : Attribute, IValidationRuleProvider
    {
        public RequiredRuleAttribute() : this(Resources.InputRequired) { }
        public RequiredRuleAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public string ErrorMessage { get; private set; }


        public virtual ValidationRule Provide(IBindingBehavior bindingBehavior)
        {
            var rule = new RequiredRule();
            rule.ErrorContent = ErrorMessage;
            return rule;
        }
    }

    public class RequiredRule : ValidationRule
    {
        public object ErrorContent
        {
            get { return errorContent; }
            set { errorContent = value; }
        }
        private object errorContent = "Required fields.";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, ErrorContent);
            if (value is string && string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, ErrorContent);
            return new ValidationResult(true, null);
        }
    }

}
