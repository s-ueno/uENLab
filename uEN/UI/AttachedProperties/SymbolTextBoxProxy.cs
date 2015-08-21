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

            textBox.Padding = new Thickness(2, 2, 20, 2);

            var symbolText = new TextBlock()
            {
                Text = Convert.ToChar(GetSymbol(textBox)).ToString(),
            };

            symbolText.SetResourceReference(TextBlock.StyleProperty, "SegoeUISymbolTextBlockKey");
            button.Content = symbolText;

            button.Visibility = Visibility.Visible;
            button.Click -= OnButtonClick;
            button.Click += OnButtonClick;

            textBox.KeyDown -= OnKeyDown;
            textBox.KeyDown += OnKeyDown;  
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

        public static readonly RoutedEvent EnterEvent = EventManager.RegisterRoutedEvent(
            "Enter", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SymbolTextBoxProxy));
        private static void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var obj = (TextBox)sender;
                var be = BindingOperations.GetBindingExpression(obj, TextBox.TextProperty);
                if (be != null)
                    be.UpdateSource();
                obj.RaiseEvent(new RoutedEventArgs(SymbolTextBoxProxy.EnterEvent));
            }
        }        

    }
}
