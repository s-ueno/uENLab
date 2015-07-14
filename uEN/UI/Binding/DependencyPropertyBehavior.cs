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
using uEN.Extensions;

namespace uEN.UI.DataBinding
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
        public Binding Binding { get; private set; }
        public BindingExpression BindingExpression { get; protected set; }
        public virtual void Ensure()
        {
            Binding = new System.Windows.Data.Binding(LambdaExpression.ToSymbol());
            Attributes = ListAttributes();

            Binding.StringFormat = GetStringFormat();
            Binding.Converter = BindingPolicy.Converter;
            Binding.ConverterParameter = BindingPolicy.ConverterParameter ?? Element;
            Binding.Mode = BindingPolicy.BindingMode;
            Binding.ValidatesOnDataErrors = BindingPolicy.ValidatesOnDataErrors;
            Binding.ValidatesOnExceptions = BindingPolicy.ValidatesOnExceptions;
            Binding.ValidatesOnNotifyDataErrors = BindingPolicy.ValidatesOnNotifyDataErrors;
            if (BindingPolicy.UpdateSourceTrigger.HasValue)
                Binding.UpdateSourceTrigger = BindingPolicy.UpdateSourceTrigger.Value;
            if (BindingPolicy.Delay.HasValue)
                Binding.Delay = BindingPolicy.Delay.Value;
            if (BindingPolicy.BindsDirectlyToSource.HasValue)
                Binding.BindsDirectlyToSource = BindingPolicy.BindsDirectlyToSource.Value;
            if (BindingPolicy.FallbackValue != null)
                Binding.FallbackValue = BindingPolicy.FallbackValue;
            if (BindingPolicy.IsAsync.HasValue)
                Binding.IsAsync = BindingPolicy.IsAsync.Value;
            if (BindingPolicy.NotifyOnSourceUpdated.HasValue)
                Binding.NotifyOnSourceUpdated = BindingPolicy.NotifyOnSourceUpdated.Value;
            if (BindingPolicy.NotifyOnTargetUpdated.HasValue)
                Binding.NotifyOnTargetUpdated = BindingPolicy.NotifyOnTargetUpdated.Value;
            if (BindingPolicy.NotifyOnValidationError.HasValue)
                Binding.NotifyOnValidationError = BindingPolicy.NotifyOnValidationError.Value;
            if (BindingPolicy.RelativeSource != null)
                Binding.RelativeSource = BindingPolicy.RelativeSource;
            if (BindingPolicy.TargetNullValue != null)
                Binding.TargetNullValue = BindingPolicy.TargetNullValue;
            if (BindingPolicy.UpdateSourceExceptionFilter != null)
                Binding.UpdateSourceExceptionFilter = BindingPolicy.UpdateSourceExceptionFilter;
            foreach (var rule in GetValidationRules())
            {
                Binding.ValidationRules.Add(rule);
            }
            SetupTemplateSelecter();
            BindingAttributes(Attributes);
            BindingExpression = BindingOperations.SetBinding(Element, DependencyProperty, Binding) as BindingExpression;
        }

        protected virtual void SetupTemplateSelecter()
        {
            if (Element is TabControl)
            {
                var tab = Element as TabControl;
                tab.ContentTemplateSelector = new ViewDataTemplateSelector();
            }
            else if (Element is ContentPresenter)
            {
                (Element as ContentPresenter).ContentTemplateSelector = new ViewDataTemplateSelector();
            }

        }

        protected virtual void BindingAttributes(IEnumerable<Attribute> atts)
        {
            foreach (var each in atts.OfType<IBindingAttribute>())
            {
                each.Binding(this);
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

            foreach (var each in Attributes.OfType<System.ComponentModel.DataAnnotations.ValidationAttribute>())
            {
                yield return new DataAnnotationRule(each);
            }

            foreach (var each in Attributes.OfType<IValidationRuleProvider>())
            {
                yield return each.Provide(this);
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

        public event EventHandler<System.EventArgs> UpdatingSource;
        public virtual void UpdateSource()
        {
            if (BindingExpression == null) return;
            if (UpdatingSource != null)
                UpdatingSource(this, new EventArgs());
            BindingExpression.UpdateSource();
        }

        public event EventHandler<System.EventArgs> UpdatingTarget;
        public virtual void UpdateTarget()
        {
            if (BindingExpression == null) return;
            if (UpdatingTarget != null)
                UpdatingTarget(this, new EventArgs());
            BindingExpression.UpdateTarget();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Element != null && DependencyProperty != null)
                    BindingOperations.ClearBinding(Element, DependencyProperty);

                Element = null;
                LambdaExpression = null;
                ViewModel = null;
                Attributes = null;
                Binding = null;
                BindingExpression = null;
            }
            disposed = true;
        }
        bool disposed = false;

    }



}
