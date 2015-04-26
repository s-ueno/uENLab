using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace uEN.UI.AttachedProperties
{
    public class SymbolTextBoxProxy
    {
        public static Symbols GetSymbol(DependencyObject obj)
        {
            return (Symbols)obj.GetValue(SymbolProperty);
        }

        public static void SetSymbol(DependencyObject obj, Symbols value)
        {
            obj.SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.RegisterAttached("Symbol", typeof(Symbols?), typeof(SymbolTextBoxProxy)
            , new PropertyMetadata(null, OnSymbolChanged));
        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null) return;
            textBox.Loaded -= OnTextBoxLoaded;
            textBox.Loaded += OnTextBoxLoaded;
        }

        static void OnTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var button = textBox.Template.FindName("SymbolButton", textBox) as Button;
            if (button == null) return;

            button.Content = Convert.ToChar(GetSymbol(textBox));
            button.Visibility = Visibility.Visible;
            button.Click -= OnButtonClick;
            button.Click += OnButtonClick;

        }
        static void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var textBox = button.FindVisualParent<TextBox>();
            if (textBox == null) return;

            //textBox.RaiseEvent(e);
            textBox.RaiseEvent(new RoutedEventArgs(ClickEvent));
        }
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SymbolTextBoxProxy));

    }
}
