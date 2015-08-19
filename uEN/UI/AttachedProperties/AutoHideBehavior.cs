using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace uEN.UI.AttachedProperties
{
    public class AutoHideBehavior
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(AutoHideBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null)
                return;

            if (true.Equals(e.OldValue))
            {
                fe.Opacity = 1d;
                fe.RemoveHandler(FrameworkElement.MouseEnterEvent, new MouseEventHandler(OnMouseEnter));
                fe.RemoveHandler(FrameworkElement.MouseLeaveEvent, new MouseEventHandler(OnMouseLeave));

            }
            if (true.Equals(e.NewValue))
            {
                //Control c = fe as Control;
                //if (c != null && c.Background == null)
                //    c.Background = Brushes.Transparent;

                //var p = fe as Panel;
                //if (p != null && p.Background == null)
                //    p.Background = Brushes.Transparent;
                fe.Opacity = 0d;
                fe.AddHandler(FrameworkElement.MouseEnterEvent, new MouseEventHandler(OnMouseEnter), true);
                fe.AddHandler(FrameworkElement.MouseLeaveEvent, new MouseEventHandler(OnMouseLeave), true);
            }
        }

        private static void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var fe = sender as FrameworkElement;
            //    fe.Opacity = 0d;
            var s = new Storyboard();
            var da = new DoubleAnimation();
            Storyboard.SetTarget(da, fe);
            Storyboard.SetTargetProperty(da, new PropertyPath(FrameworkElement.OpacityProperty));
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            s.Children.Add(da);




            s.Begin();
        }

        private static void OnMouseEnter(object sender, MouseEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var s = new Storyboard();
            var da = new DoubleAnimation();
            Storyboard.SetTarget(da, fe);
            Storyboard.SetTargetProperty(da, new PropertyPath(FrameworkElement.OpacityProperty));
            da.From = 0d;
            da.To = 1d;
            da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            s.Children.Add(da);


            s.Begin();
        }

    }


}
