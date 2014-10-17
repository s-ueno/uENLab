using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace uEN.UI.Binding
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

        public BindingBehaviorBuilder<T> StringFormat(string value)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.StringFormat = value;
            return this;
        }

        public BindingBehaviorBuilder<T> Converter(IValueConverter converter)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            current.BindingPolicy.Converter = converter;
            return this;
        }

        public BindingBehaviorBuilder<T> Convert(
            Func<object, Type, object, CultureInfo, object> convert = null,
            Func<object, Type, object, CultureInfo, object> convertBack = null)
        {
            var current = ValidateCurrentBehavior<DependencyPropertyBehavior>();
            var converter = new SimpleValueConverter()
            {
                ConvertMethod = convert,
                ConvertBackMethod = convertBack
            };
            current.BindingPolicy.Converter = converter;
            return this;
        }

        private T ValidateCurrentBehavior<T>() where T : IBindingBehavior
        {
            var current = CurrentBehavior;
            if (!(current is T))
                throw new InvalidOperationException(string.Format("CurrentBehavior is not {0}", typeof(T).Name));
            return (T)current;
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
