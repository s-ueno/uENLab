using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace uEN.UI.DataBinding
{
    public interface IBindingBehaviorBuilder
    {
        BizView View { get; }
        object ViewModel { get; }
    }

    public class BindingBehaviorBuilder<T> : IBindingBehaviorBuilder where T : BizViewModel
    {
        public BindingBehaviorBuilder(BizView view)
        {
            View = view;
        }
        public BizView View { get; private set; }

        public object ViewModel
        {
            get
            {
                if (View == null)
                {
                    return null;
                }
                return View.DataContext;
            }
        }

        protected DependencyObject CurrentElement { get; private set; }
        protected IBindingBehavior CurrentBehavior { get; private set; }

        public BindingBehaviorBuilder<T> Element(DependencyObject element)
        {
            CurrentElement = element;
            return this;
        }

        public BindingBehaviorBuilder<T> Binding<P>(DependencyProperty dependencyProperty, Expression<Func<T, P>> property,
            BindingMode mode = BindingMode.Default, UpdateSourceTrigger? updateSourceTrigger = null)
        {
            var behavior = NewAddBehavior<DependencyPropertyBehavior>();

            behavior.ViewModel = ViewModel;
            behavior.Element = CurrentElement;
            behavior.DependencyProperty = dependencyProperty;
            behavior.LambdaExpression = property;
            behavior.BindingPolicy.BindingMode = mode;
            behavior.BindingPolicy.UpdateSourceTrigger = updateSourceTrigger;

            return this;
        }
        public BindingBehaviorBuilder<T> ValidatesOnDataErrors(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.ValidatesOnDataErrors = value;
            return this;
        }
        public BindingBehaviorBuilder<T> ValidatesOnExceptions(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.ValidatesOnExceptions = value;
            return this;
        }
        public BindingBehaviorBuilder<T> ValidatesOnNotifyDataErrors(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.ValidatesOnNotifyDataErrors = value;
            return this;
        }
        public BindingBehaviorBuilder<T> StringFormat(string value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.StringFormat = value;
            return this;
        }
        public BindingBehaviorBuilder<T> BindsDirectlyToSource(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.BindsDirectlyToSource = value;
            return this;
        }
        public BindingBehaviorBuilder<T> Delay(int value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.Delay = value;
            return this;
        }
        public BindingBehaviorBuilder<T> FallbackValue(object value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.FallbackValue = value;
            return this;
        }
        public BindingBehaviorBuilder<T> IsAsync(bool value = true)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.IsAsync = value;
            return this;
        }
        public BindingBehaviorBuilder<T> NotifyOnSourceUpdated(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.NotifyOnSourceUpdated = value;
            return this;
        }
        public BindingBehaviorBuilder<T> NotifyOnTargetUpdated(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.NotifyOnTargetUpdated = value;
            return this;
        }
        public BindingBehaviorBuilder<T> NotifyOnValidationError(bool value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.NotifyOnValidationError = value;
            return this;
        }
        public BindingBehaviorBuilder<T> RelativeSource(RelativeSource value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.RelativeSource = value;
            return this;
        }
        public BindingBehaviorBuilder<T> TargetNullValue(object value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.TargetNullValue = value;
            return this;
        }
        public BindingBehaviorBuilder<T> UpdateSourceExceptionFilter(UpdateSourceExceptionFilterCallback value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.UpdateSourceExceptionFilter = value;
            return this;
        }
        public BindingBehaviorBuilder<T> AddValidationRule(ValidationRule value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.ValidationRules.Add(value);
            return this;
        }




        public BindingBehaviorBuilder<T> Converter(IValueConverter converter, object parameter = null)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.Converter = converter;
            current.BindingPolicy.ConverterParameter = parameter;
            return this;
        }
        public BindingBehaviorBuilder<T> Convert(
            Func<object, Type, object, CultureInfo, object> convert = null,
            Func<object, Type, object, CultureInfo, object> convertBack = null, 
            object parameter = null)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            var converter = new SimpleValueConverter()
            {
                ConvertMethod = convert,
                ConvertBackMethod = convertBack,
            };
            current.BindingPolicy.Converter = converter;
            current.BindingPolicy.ConverterParameter = parameter;
            return this;
        }

        private TBehavior ValidateCurrentBehavior<TBehavior>() where TBehavior : IBindingBehavior
        {
            var current = CurrentBehavior;
            if (!(current is TBehavior))
                throw new InvalidOperationException(string.Format("CurrentBehavior is not {0}", typeof(T).Name));
            return (TBehavior)current;
        }

        public BindingBehaviorBuilder<T> Binding(RoutedEvent routedEvent, Expression<Func<T, Action>> @event)
        {
            var behavior = NewAddBehavior<RoutedEventBehavior>();

            behavior.ViewModel = ViewModel;
            behavior.Element = CurrentElement;
            behavior.RoutedEvent = routedEvent;
            behavior.LambdaExpression = @event;

            return this;
        }

        public BindingBehaviorBuilder<T> Binding(RoutedEvent routedEvent, Expression<Func<T, Action<RoutedEventArgs>>> @event)
        {
            var behavior = NewAddBehavior<RoutedEventBehavior>();

            behavior.ViewModel = ViewModel;
            behavior.Element = CurrentElement;
            behavior.RoutedEvent = routedEvent;
            behavior.LambdaExpression = @event;

            return this;
        }

        public TBehavior NewAddBehavior<TBehavior>() where TBehavior : IBindingBehavior, new()
        {
            var behavior = new TBehavior();
            CurrentBehavior = behavior;
            View.BindingBehaviors.Add(behavior);
            return behavior;
        }
    }
}
