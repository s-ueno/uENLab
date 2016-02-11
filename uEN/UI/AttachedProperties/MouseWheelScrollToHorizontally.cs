using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace uEN.UI.AttachedProperties
{
    public class MouseWheelScrollToHorizontally
    {
        public static MouseWheelScrollToHorizontally GetInstance(DependencyObject obj)
        {
            return (MouseWheelScrollToHorizontally)obj.GetValue(InstanceProperty);
        }
        public static void SetInstance(DependencyObject obj, MouseWheelScrollToHorizontally value)
        {
            obj.SetValue(InstanceProperty, value);
        }
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.RegisterAttached("Instance", typeof(MouseWheelScrollToHorizontally), typeof(MouseWheelScrollToHorizontally),
            new FrameworkPropertyMetadata(null, OnInstanceChanged));
        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = d as ScrollViewer;
            if (host == null) return;

            var oldBehavior = e.OldValue as MouseWheelScrollToHorizontally;
            if (oldBehavior != null)
            {
                oldBehavior.UnRegister();
            }

            var me = e.NewValue as MouseWheelScrollToHorizontally;
            if (me != null)
                me.Register(host);
        }

        public void Register(ScrollViewer d)
        {
            UnRegister();

            ScrollHost = d;
            ScrollHost.Unloaded -= OnScrollHostUnloaded;
            ScrollHost.Unloaded += OnScrollHostUnloaded;

            ScrollHost.PreviewMouseWheel -= OnScrollHostPreviewMouseWheel;
            ScrollHost.PreviewMouseWheel += OnScrollHostPreviewMouseWheel;
        }
        private void OnScrollHostUnloaded(object sender, RoutedEventArgs e)
        {
            UnRegister();
        }
        public void UnRegister()
        {
            if (ScrollHost != null)
            {
                ScrollHost.Unloaded -= OnScrollHostUnloaded;
                ScrollHost.PreviewMouseWheel -= OnScrollHostPreviewMouseWheel;
            }
            ScrollHost = null;
        }
        private void OnScrollHostPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
        protected ScrollViewer ScrollHost { get; set; }
    }
}
