using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uEN.UI.Validation
{
    public class DataAnnotationRule : System.Windows.Controls.ValidationRule
    {
        public DataAnnotationRule(System.ComponentModel.DataAnnotations.ValidationAttribute att)
        {
            Validation = att;
        }
        public System.ComponentModel.DataAnnotations.ValidationAttribute Validation { get; private set; }

        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var ret = Validation.IsValid(value);
            return new System.Windows.Controls.ValidationResult(ret,  Validation.ErrorMessage);
        }
    }
}
