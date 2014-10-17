using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI.Validation;


namespace uEN.UI.Binding
{
    public class DependencyPropertyBehavior : IBindingBehavior
    {
        public BindingPolicy BindingPolicy { get; set; }
        public DependencyPropertyBehavior()
        {
            BindingPolicy = new BindingPolicy();
        }

        public DependencyProperty DependencyProperty { get; set; }
        public object ViewModel { get; set; }
        public DependencyObject Element { get; set; }
        public LambdaExpression LambdaExpression { get; set; }
        protected IEnumerable<Attribute> Attributes { get; set; }

        public void Ensure()
        {
            var binding = new System.Windows.Data.Binding(LambdaExpression.ToSymbol());
            Attributes = LambdaExpression.ListAttributes();

            binding.StringFormat = GetStringFormat();
            binding.Converter = BindingPolicy.Converter;
            binding.Mode = BindingPolicy.BindingMode;
            if (BindingPolicy.UpdateSourceTrigger.HasValue)
                binding.UpdateSourceTrigger = BindingPolicy.UpdateSourceTrigger.Value;

            binding.ValidatesOnDataErrors = BindingPolicy.ValidatesOnDataErrors;
            binding.ValidatesOnExceptions = BindingPolicy.ValidatesOnExceptions;
            binding.ValidatesOnNotifyDataErrors = BindingPolicy.ValidatesOnNotifyDataErrors;

            foreach (var rule in GetValidationRules())
            {
                binding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(Element, DependencyProperty, binding);
        }

        protected virtual IEnumerable<ValidationRule> GetValidationRules()
        {
            foreach (var ruleProvider in Attributes.OfType<IValidationRuleProvider>())
            {
                yield return ruleProvider.Provide();
            }
        }

        protected virtual string GetStringFormat()
        {
            if (!string.IsNullOrWhiteSpace(BindingPolicy.StringFormat))
                return BindingPolicy.StringFormat;

            var format = Attributes.FirstOrDefault(x => x is BindingStringFormatAttribute) as BindingStringFormatAttribute;
            return format != null ? format.Value : null;
        }

    }

    public class SampleError : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return new ValidationResult(false, "aaaaa");
        }
    }

}
