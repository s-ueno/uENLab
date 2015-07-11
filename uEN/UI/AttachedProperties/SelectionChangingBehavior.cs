using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uEN.Core;
using uEN.UI.AttachedProperties;

namespace uEN.UI.AttachedProperties
{
    public delegate void SelectionChangingEventHandler(object sender, SelectionChangingEventargs e);
    public class SelectionChangingEventargs : RoutedEventArgs
    {
        public SelectionChangingEventargs(object oldItem, object newItem)
        {
            OldItem = oldItem;
            NewItem = NewItem;
        }
        public bool Cancel { get; set; }
        public object OldItem { get; private set; }
        public object NewItem { get; private set; }
    }

    public class SelectionChangingBehavior
    {
        public static readonly RoutedEvent SelectionChangingEvent =
            EventManager.RegisterRoutedEvent("SelectionChanging", RoutingStrategy.Direct,
            typeof(SelectionChangingEventHandler), typeof(SelectionChangingBehavior));
        public static SelectionChangingBehavior GetInstance(DependencyObject obj)
        {
            return (SelectionChangingBehavior)obj.GetValue(InstanceProperty);
        }
        public static void SetInstance(DependencyObject obj, SelectionChangingBehavior value)
        {
            obj.SetValue(InstanceProperty, value);
        }


        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.RegisterAttached("Instance", typeof(SelectionChangingBehavior), typeof(SelectionChangingBehavior),
            new FrameworkPropertyMetadata(null, OnInstanceChanged));
        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as Selector;
            if (selector == null) return;

            var oldValue = e.OldValue as SelectionChangingBehavior;
            if (oldValue != null)
            {
                oldValue.UnRegister(selector);
            }

            var newValue = e.NewValue as SelectionChangingBehavior;
            if (newValue != null)
            {
                newValue.Register(selector);
            }
        }
        public void Register(Selector selector)
        {
            selector.SelectionChanged += OnSelectionChanged;
        }
        public void UnRegister(Selector selector)
        {
            selector.SelectionChanged -= OnSelectionChanged;
        }
        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (engaged) return;

            var selector = (Selector)sender;
            if (!selector.IsLoaded) return;

            engaged = true;
            try
            {
                var newItem = e.AddedItems.OfType<object>().FirstOrDefault();
                var oldItem = e.RemovedItems.OfType<object>().FirstOrDefault();

                //selector.SelectedItem = oldItem;
                selector.SetCurrentValue(Selector.SelectedItemProperty, oldItem);

                var eventArgs = new SelectionChangingEventargs(oldItem, newItem) { RoutedEvent = SelectionChangingEvent, };
                selector.RaiseEvent(eventArgs);
                if (!eventArgs.Cancel)
                {
                    //selector.SelectedItem = newItem;
                    selector.SetCurrentValue(Selector.SelectedItemProperty, newItem);
                }
            }
            finally
            {
                engaged = false;
            }
        }
        bool engaged = false;
    }
}
