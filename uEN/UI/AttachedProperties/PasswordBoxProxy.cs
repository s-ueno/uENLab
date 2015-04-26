using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace uEN.UI.AttachedProperties
{
    public class PasswordBoxProxy
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(PasswordBoxProxy.IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PasswordBoxProxy), 
            new UIPropertyMetadata(false, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as PasswordBox;
            if (p == null)
                return;

            var value = e.NewValue as bool?;
            if (!value.HasValue || value.Value == false)
            {
                p.PasswordChanged -= OnPasswordChanged;
                p.KeyDown -= OnKeyDown;   
                return;
            }
                
            p.PasswordChanged -= OnPasswordChanged;
            p.PasswordChanged += OnPasswordChanged;

            p.KeyDown -= OnKeyDown;   
            p.KeyDown += OnKeyDown;   
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(PasswordBoxProxy),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var obj = sender as PasswordBox;
            if (null == obj) return;
            obj.SetCurrentValue(PasswordBoxProxy.TextProperty, obj.Password);            
        }


        public static readonly RoutedEvent EnterActionEvent = EventManager.RegisterRoutedEvent(
            "EnterAction", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(PasswordBoxProxy));
        private static void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var obj = (PasswordBox)sender;
                var be = BindingOperations.GetBindingExpression(obj, PasswordBoxProxy.TextProperty);
                if (be != null)
                    be.UpdateSource();
                obj.RaiseEvent(new RoutedEventArgs(PasswordBoxProxy.EnterActionEvent));
            }            
        }
    }
}
