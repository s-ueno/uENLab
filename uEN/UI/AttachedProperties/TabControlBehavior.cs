using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using uEN;
using uEN.UI;
namespace uEN.UI.AttachedProperties
{
    public enum TabStyle
    {
        None, Modern, Basic
    }

    public class TabControlBehavior : FrameworkElement
    {
        public static Visibility GetUnderlineVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(UnderlineVisibilityProperty);
        }
        public static void SetUnderlineVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(UnderlineVisibilityProperty, value);
        }
        public static readonly DependencyProperty UnderlineVisibilityProperty =
            DependencyProperty.RegisterAttached("UnderlineVisibility", typeof(Visibility), typeof(TabControlBehavior),
            new PropertyMetadata(Visibility.Visible));

        public static TabStyle GetTabStyle(DependencyObject obj)
        {
            return (TabStyle)obj.GetValue(TabStyleProperty);
        }
        public static void SetTabStyle(DependencyObject obj, TabStyle value)
        {
            obj.SetValue(TabStyleProperty, value);
        }
        public static readonly DependencyProperty TabStyleProperty =
            DependencyProperty.RegisterAttached("TabStyle", typeof(TabStyle), typeof(TabControlBehavior)
            , new FrameworkPropertyMetadata(TabStyle.None, OnTabStyleChanged));
        private static void OnTabStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tab = d as TabControl;
            if (tab == null) return;

            var style = GetTabStyle(tab);
            if (style == TabStyle.Basic)
            {
                tab.SetResourceReference(TabControl.StyleProperty, "BasicTab");
            }
            if (style == TabStyle.Modern)
            {
                tab.SetResourceReference(TabControl.StyleProperty, "ModernTab");
            }
        }
    }
}
