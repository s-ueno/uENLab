using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public BindingBehaviorBuilder<T> Binding(DependencyProperty dependencyProperty, Expression<Func<T, object>> property)
        {
            var behavior = NewAddBehavior<DependencyPropertyBehavior>();

            behavior.ViewModel = ViewModel;
            behavior.Element = CurrentElement;
            behavior.DependencyProperty = dependencyProperty;
            behavior.LambdaExpression = property;

            return this;
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
