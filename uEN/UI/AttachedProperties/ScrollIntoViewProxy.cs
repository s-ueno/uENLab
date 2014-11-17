using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace uEN.UI.AttachedProperties
{
    public class ScrollIntoViewProxy
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ScrollIntoViewProxy), new UIPropertyMetadata(false, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as ListBox;
            var value = e.NewValue as bool?;
            if (listBox != null)
            {
                listBox.SelectionChanged -= listBox_SelectionChanged;
                if (value == true)
                {
                    listBox.SelectionChanged += listBox_SelectionChanged;
                }
            }
        }

        static void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }
    }
}
