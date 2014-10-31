using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public IEnumerable<Attribute> Attributes { get; protected set; }
        public BindingExpressionBase BindingExpression { get; protected set; }
        public virtual void Ensure()
        {
            var binding = new System.Windows.Data.Binding(LambdaExpression.ToSymbol());
            Attributes = ListAttributes();

            binding.StringFormat = GetStringFormat();
            binding.Converter = BindingPolicy.Converter;
            binding.Mode = BindingPolicy.BindingMode;
            binding.ValidatesOnDataErrors = BindingPolicy.ValidatesOnDataErrors;
            binding.ValidatesOnExceptions = BindingPolicy.ValidatesOnExceptions;
            binding.ValidatesOnNotifyDataErrors = BindingPolicy.ValidatesOnNotifyDataErrors;
            if (BindingPolicy.UpdateSourceTrigger.HasValue)
                binding.UpdateSourceTrigger = BindingPolicy.UpdateSourceTrigger.Value;
            if (BindingPolicy.Delay.HasValue)
                binding.Delay = BindingPolicy.Delay.Value;
            if (BindingPolicy.BindsDirectlyToSource.HasValue)
                binding.BindsDirectlyToSource = BindingPolicy.BindsDirectlyToSource.Value;
            if (BindingPolicy.FallbackValue != null)
                binding.FallbackValue = BindingPolicy.FallbackValue;
            if (BindingPolicy.IsAsync.HasValue)
                binding.IsAsync = BindingPolicy.IsAsync.Value;
            if (BindingPolicy.NotifyOnSourceUpdated.HasValue)
                binding.NotifyOnSourceUpdated = BindingPolicy.NotifyOnSourceUpdated.Value;
            if (BindingPolicy.NotifyOnTargetUpdated.HasValue)
                binding.NotifyOnTargetUpdated = BindingPolicy.NotifyOnTargetUpdated.Value;
            if (BindingPolicy.NotifyOnValidationError.HasValue)
                binding.NotifyOnValidationError = BindingPolicy.NotifyOnValidationError.Value;
            if (BindingPolicy.RelativeSource != null)
                binding.RelativeSource = BindingPolicy.RelativeSource;
            if (BindingPolicy.TargetNullValue != null)
                binding.TargetNullValue = BindingPolicy.TargetNullValue;
            if (BindingPolicy.UpdateSourceExceptionFilter != null)
                binding.UpdateSourceExceptionFilter = BindingPolicy.UpdateSourceExceptionFilter;
            foreach (var rule in GetValidationRules())
            {
                binding.ValidationRules.Add(rule);
            }
            BindingExpression = BindingOperations.SetBinding(Element, DependencyProperty, binding);
            SetupTemplateSelecter();
        }

        protected virtual void SetupTemplateSelecter()
        {
            if (Element is TabControl)
            {
                var tab = Element as TabControl;
                tab.ContentTemplateSelector = new ViewDataTemplateSelector();
            }
        }


        protected virtual IEnumerable<Attribute> ListAttributes()
        {
            var t = ViewModel.GetType();
            var classAtts = t.GetCustomAttributes(true).OfType<Attribute>();
            var asmAtts = t.Assembly.GetCustomAttributes(true).OfType<Attribute>();
            var pAtts = LambdaExpression.ListAttributes();

            var list = classAtts.Union(asmAtts).Union(pAtts).ToArray();
            foreach (var each in list)
            {
                yield return each;
            }
        }

        protected virtual IEnumerable<ValidationRule> GetValidationRules()
        {
            foreach (var each in BindingPolicy.ValidationRules)
            {
                yield return each;
            }
            foreach (var each in Attributes.OfType<IValidationRuleProvider>())
            {
                yield return each.Provide();
            }
        }
        protected virtual string GetStringFormat()
        {
            if (!string.IsNullOrWhiteSpace(BindingPolicy.StringFormat))
                return BindingPolicy.StringFormat;

            var format = Attributes.FirstOrDefault(x => x is BindingStringFormatAttribute) as BindingStringFormatAttribute;
            return format != null ? format.Value : null;
        }
        public virtual bool HasValidationError
        {
            get
            {
                return BindingExpression != null && BindingExpression.HasValidationError ? true : false;
            }
        }
        public virtual ReadOnlyCollection<ValidationError> ValidationErrors
        {
            get
            {
                if (BindingExpression == null || BindingExpression.ValidationErrors == null)
                {
                    return new ReadOnlyCollection<ValidationError>(new List<ValidationError>());
                }
                return BindingExpression.ValidationErrors;
            }
        }
        public virtual void UpdateSource()
        {
            if (BindingExpression == null) return;
            BindingExpression.UpdateSource();
        }
        public virtual void UpdateTarget()
        {
            if (BindingExpression == null) return;
            BindingExpression.UpdateTarget();
        }

    }



}
