﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace uEN.UI.Validation
{

    public class RequiredRuleAttribute : Attribute, IValidationRuleProvider
    {
        public RequiredRuleAttribute() : this("入力してください。") { }
        public RequiredRuleAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public string ErrorMessage { get; private set; }


        ValidationRule IValidationRuleProvider.Provide()
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
